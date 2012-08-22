using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.TextConverter
{
    /// <summary>
    /// Represents a word in the text.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Empty word like string.Empty
        /// </summary>
        public static readonly Word Empty = new Word(string.Empty, 0, 0);

        
        /// <summary>
        /// Default initialization.
        /// </summary>
        public Word(string text) 
        {
            Text = text;
        }


        /// <summary>
        /// Initialize with text and position information.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="line"></param>
        /// <param name="charPos"></param>
        public Word(string text, int line, int charPos)
        {
            Text = text;
            Line = line;
            LineCharPosition = charPos;
        }


        /// <summary>
        /// The raw text of the word.
        /// </summary>
        public string Text;
        

        /// <summary>
        /// The line number this word was found at.
        /// </summary>
        public int Line;


        /// <summary>
        /// The char position in the line that this word was found at.
        /// </summary>
        public int LineCharPosition;
    }
}
