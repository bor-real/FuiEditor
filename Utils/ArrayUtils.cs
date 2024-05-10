namespace FuiEditor.Utils
{
     static class ArrayUtils
     {
           //https://teratail.com/questions/20408
           //author: yuba, thx!

           /// <summary>
           /// Searches for a sequence that matches a byte pattern within a byte array.
           /// The algorithm used is the Sunday method.
           /// </summary>
           /// <param name="pattern">The pattern to search for</param>
           /// <param name="text">The array to search in</param>
           /// <returns>The start position if found, or -1 if not found</returns>
           public static int SearchBytes(byte[] text, int startIndex, byte[] pattern)
           {
               int patternLen = pattern.Length, textLen = text.Length;

               // Create a shift table
               int[] qs_table = new int[byte.MaxValue + 1];

               // For the default case (when a character not present in the pattern is found immediately after the search range),
               // the next search range is the next character (i.e., the shift width is the pattern length + 1).
               for (int i = qs_table.Length; i-- > 0;) qs_table[i] = patternLen + 1;

               // If a character present in the pattern is found immediately after the search range,
               // the next search range is the position where that character and the character in the pattern match.
               for (int n = 0; n < patternLen; ++n) qs_table[pattern[n]] = patternLen - n;

               int pos;

               // Use the shift table to compare the text up to the end of the text
               for (pos = startIndex; pos < textLen - patternLen; pos += qs_table[text[pos + patternLen]])
               {
                   // Compare for a match. If there is a match, return the current comparison position.
                   if (CompareBytes(text, pos, pattern, patternLen)) return pos;
               }

               // If the end of the text has not yet been compared, compare it as well
               if (pos == textLen - patternLen)
               {
                   // Compare for a match. If there is a match, return the current comparison position.
                   if (CompareBytes(text, pos, pattern, patternLen)) return pos;
               }

               // No matching position was found.
               return -1;
           }

           /// <summary>
           /// Determines whether one array (pattern) is contained within another array (text).
           /// 
           /// An ArrayOutOfBoundException will occur if
           /// pos + patternLen is greater than text.Length,
           /// or if pos or patternLen is less than 0,
           /// or if needdleLen is greater than pattern.Length.
           /// </summary>
           /// <param name="text">Compare this array from pos with pattern</param>
           /// <param name="pos">Where to start the comparison in text</param>
           /// <param name="pattern">This entire array is checked to see if it matches text from pos</param>
           /// <param name="patternLen">The length of pattern to check for a match</param>
           /// <returns></returns>
           static bool CompareBytes(byte[] text, int pos, byte[] pattern, int patternLen)
           {
               for (int comparer = 0; comparer < patternLen; ++comparer)
               {
                   if (text[comparer + pos] != pattern[comparer]) return false;
               }
               
               return true;
           }
   }
}
