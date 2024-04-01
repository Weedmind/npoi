/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.OpenXml4Net.OPC;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.Util;

namespace NPOI.OpenXml4Net.OPC.Internal.Marshallers
{
    /// <summary>
    /// Zip part marshaller. This marshaller is use to save any part in a zip stream.
    /// </summary>
    /// <remarks>
    /// @author Julien Chable
    /// </remarks>

    public class ZipPartMarshaller : PartMarshaller
    {
        private static POILogger logger = POILogFactory.GetLogger(typeof(ZipPartMarshaller));

        /// <summary>
        /// Save the specified part.
        /// </summary>
        /// <exception cref="OpenXml4NetException">OpenXml4NetException
        /// Throws if an internal exception is thrown.
        /// </exception>
        public bool Marshall(PackagePart part, Stream os)
        {
            if (!(os is ZipOutputStream))
            {
                logger.Log(POILogger.ERROR,"Unexpected class " + os.GetType().Name);
                throw new OpenXml4NetException("ZipOutputStream expected !");
                // Normally should happen only in developement phase, so just throw
                // exception
            }

            // check if there is anything to save for some parts. We don't do this for all parts as some code
            // might depend on empty parts being saved, e.g. some unit tests verify this currently.
            if (part.Size == 0 && part.PartName.Name.Equals("/xl/sharedStrings.xml"))
            {
                return true;
            }

            ZipOutputStream zos = (ZipOutputStream)os;
            string name = ZipHelper
                    .GetZipItemNameFromOPCName(part.PartName.URI
                            .OriginalString);
            ZipEntry partEntry = new ZipEntry(name);
            try
            {
                // Create next zip entry
                zos.PutNextEntry(partEntry);

                // Saving data in the ZIP file
                Stream ins = part.GetInputStream();
                byte[] buff = new byte[ZipHelper.READ_WRITE_FILE_BUFFER_SIZE];
                int totalRead = 0;
                while (true)
                {
                    int resultRead = ins.Read(buff, 0, buff.Length);
                    if (resultRead == 0)
                    {
                        // End of file reached
                        break;
                    }
                    zos.Write(buff, 0, resultRead);
                    totalRead += resultRead;
                }
                zos.CloseEntry();
            }
            catch (IOException ioe)
            {
                logger.Log(POILogger.ERROR, "Cannot write: " + part.PartName + ": in ZIP", ioe);
                return false;
            }

            // Saving relationship part
            if (part.HasRelationships)
            {
                PackagePartName relationshipPartName = PackagingUriHelper
                        .GetRelationshipPartName(part.PartName);

                MarshallRelationshipPart(part.Relationships,
                        relationshipPartName, zos);

            }
            return true;
        }

        /// <summary>
        /// Save relationships into the part.
        /// </summary>
        /// <param name="rels">The relationships collection to marshall.
        /// </param>
        /// <param name="relPartName"> Part name of the relationship part to marshall.
        /// </param>
        /// <param name="zos">Zip output stream in which to save the XML content of the
        /// relationships serialization.
        /// </param>
        public static bool MarshallRelationshipPart(
                PackageRelationshipCollection rels, PackagePartName relPartName,
                ZipOutputStream zos)
        {
            // Building xml
            XmlDocument xmlOutDoc = new XmlDocument();
            // make something like <Relationships
            // xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
            System.Xml.XmlNamespaceManager xmlnsManager = new System.Xml.XmlNamespaceManager(xmlOutDoc.NameTable);
            xmlnsManager.AddNamespace("x", PackageNamespaces.RELATIONSHIPS);

            XmlNode root = xmlOutDoc.AppendChild(xmlOutDoc.CreateElement(PackageRelationship.RELATIONSHIPS_TAG_NAME, PackageNamespaces.RELATIONSHIPS));

            // <Relationship
            // TargetMode="External"
            // Id="rIdx"
            // Target="http://www.custom.com/images/pic1.jpg"
            // Type="http://www.custom.com/external-resource"/>

            Uri sourcePartURI = PackagingUriHelper
                    .GetSourcePartUriFromRelationshipPartUri(relPartName.URI);

            foreach (PackageRelationship rel in rels)
            {
                // the relationship element
                XmlElement relElem = xmlOutDoc.CreateElement(PackageRelationship.RELATIONSHIP_TAG_NAME,PackageNamespaces.RELATIONSHIPS);

                // the relationship ID
                relElem.SetAttribute(PackageRelationship.ID_ATTRIBUTE_NAME, rel.Id);

                // the relationship Type
                relElem.SetAttribute(PackageRelationship.TYPE_ATTRIBUTE_NAME, rel
                        .RelationshipType);

                // the relationship Target
                String targetValue;
                Uri uri = rel.TargetUri;
                if (rel.TargetMode == TargetMode.External)
                {
                    // Save the target as-is - we don't need to validate it,
                    //  alter it etc
                    targetValue = uri.OriginalString;

                    // add TargetMode attribute (as it is external link external)
                    relElem.SetAttribute(
                            PackageRelationship.TARGET_MODE_ATTRIBUTE_NAME,
                            "External");
                }
                else
                {
                    targetValue = PackagingUriHelper.RelativizeUri(
                            sourcePartURI, rel.TargetUri, true).ToString();
                }
                relElem.SetAttribute(PackageRelationship.TARGET_ATTRIBUTE_NAME,
                        targetValue);
                xmlOutDoc.DocumentElement.AppendChild(relElem);
            }

            xmlOutDoc.Normalize();

            // String schemaFilename = Configuration.getPathForXmlSchema()+
            // File.separator + "opc-relationships.xsd";

            // Save part in zip
            ZipEntry ctEntry = new ZipEntry(ZipHelper.GetZipURIFromOPCName(
                    relPartName.URI.ToString()).OriginalString);
            try
            {
                zos.PutNextEntry(ctEntry);

                StreamHelper.SaveXmlInStream(xmlOutDoc, zos);
                zos.CloseEntry();
            }
            catch (IOException e)
            {
                logger.Log(POILogger.ERROR,"Cannot create zip entry " + relPartName, e);
                return false;
            }
            return true; // success
        }
    }
}
