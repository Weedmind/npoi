/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
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

namespace NPOI.SS.UserModel
{

    /// <summary>
    /// The enumeration value indicating horizontal alignment of a cell,
    /// i.e., whether it is aligned general, left, right, horizontally centered, Filled (replicated),
    /// justified, centered across multiple cells, or distributed.
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// The horizontal alignment is general-aligned. Text data is left-aligned.
        /// Numbers, dates, and times are right-aligned. Boolean types are centered.
        /// Changing the alignment does not change the type of data.
        /// </summary>
        General=0,

        /// <summary>
        /// The horizontal alignment is left-aligned, even in Right-to-Left mode.
        /// Aligns contents at the left edge of the cell. If an indent amount is specified, the contents of
        /// the cell is indented from the left by the specified number of character spaces. The character spaces are
        /// based on the default font and font size for the workbook.
        /// </summary>
        Left = 1,

        /// <summary>
        /// The horizontal alignment is centered, meaning the text is centered across the cell.
        /// </summary>
        Center = 2,

        /// <summary>
        /// The horizontal alignment is right-aligned, meaning that cell contents are aligned at the right edge of the cell,
        /// even in Right-to-Left mode.
        /// </summary>
        Right = 3,
        /// <summary>
        /// The horizontal alignment is justified (flush left and right).
        /// For each line of text, aligns each line of the wrapped text in a cell to the right and left
        /// (except the last line). If no single line of text wraps in the cell, then the text is not justified.
        /// </summary>
        Justify = 5,
        /// <summary>
        /// <para>
        /// Indicates that the value of the cell should be Filled
        /// across the entire width of the cell. If blank cells to the right also have the fill alignment,
        /// they are also Filled with the value, using a convention similar to centerContinuous.
        /// </para>
        /// <para>
        /// Additional rules:
        /// <list type="number">
        /// <item><description>Only whole values can be Appended, not partial values.</description></item>
        /// <item><description>The column will not be widened to 'best fit' the Filled value</description></item>
        /// <item><description>If Appending an Additional occurrence of the value exceeds the boundary of the cell
        /// left/right edge, don't append the Additional occurrence of the value.</description></item>
        /// <item><description>The display value of the cell is Filled, not the underlying raw number.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        Fill =4,



        /// <summary>
        /// The horizontal alignment is centered across multiple cells.
        /// The information about how many cells to span is expressed in the Sheet Part,
        /// in the row of the cell in question. For each cell that is spanned in the alignment,
        /// a cell element needs to be written out, with the same style Id which references the centerContinuous alignment.
        /// </summary>
        CenterSelection= 6,

        /// <summary>
        /// <para>
        /// Indicates that each 'word' in each line of text inside the cell is evenly distributed
        /// across the width of the cell, with flush right and left margins.
        /// </para>
        /// <para>
        /// When there is also an indent value to apply, both the left and right side of the cell
        /// are pAdded by the indent value.
        /// </para>
        /// <para>
        /// 
        /// A 'word' is a set of characters with no space character in them.
        /// </para>
        /// <para>
        /// Two lines inside a cell are Separated by a carriage return.
        /// </para>
        /// </summary>
        Distributed=7
    }

}