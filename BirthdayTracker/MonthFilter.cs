using System.Collections.Generic;
using System;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename:   MonthFilter.cs
   // Purpose:    Provide a centralized class for use in
   //             filtering based on months of the year.
   // Author:     Wade Rauschenbach
   // Version:    0.1.0
   // Date:       20-Aug-2020
   // Tests:      N/A
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   // 
   // [0.1.0] 20-Aug-2020
   // | [Added]
   // | - Initial MonthFilter implementation.
   /**********************************************************/
   public class MonthFilter
   {
      private static List<MonthFilter> filterList = new List<MonthFilter>();

      public static readonly MonthFilter All = new MonthFilter("All");
      public static readonly MonthFilter Jan = new MonthFilter("1 - Jan");
      public static readonly MonthFilter Feb = new MonthFilter("2 - Feb");
      public static readonly MonthFilter Mar = new MonthFilter("3 - Mar");
      public static readonly MonthFilter Apr = new MonthFilter("4 - Apr");
      public static readonly MonthFilter May = new MonthFilter("5 - May");
      public static readonly MonthFilter Jun = new MonthFilter("6 - Jun");
      public static readonly MonthFilter Jul = new MonthFilter("7 - Jul");
      public static readonly MonthFilter Aug = new MonthFilter("8 - Aug");
      public static readonly MonthFilter Sep = new MonthFilter("9 - Sep");
      public static readonly MonthFilter Oct = new MonthFilter("10 - Oct");
      public static readonly MonthFilter Nov = new MonthFilter("11 - Nov");
      public static readonly MonthFilter Dec = new MonthFilter("12 - Dec");

      public string Text { get; private set; }
      public int Month { get; private set; }

      /**********************************************************/
      // Method:  private MonthFilter (string text)
      // Purpose: Creates a new instance of MonthFilter with the
      //          specified 'text' and receives the next 'Index'.
      // Inputs:  string text
      /**********************************************************/
      private MonthFilter(string text)
      {
         Text = text;
         Month = filterList.Count;
         filterList.Add(this);
      }

      /**********************************************************/
      // Method:  public MonthFilter ()
      // Purpose: Gets the next MonthFilter after this.
      //          e.g. from ('8 - Aug') to ('9 - Sep') or
      //          from ('12 - Dec') to ('All').
      // Returns: The next MonthFilter based on the 'Month'.
      // Outputs: MonthFilter
      /**********************************************************/
      public MonthFilter GetNext()
      {
         return filterList[(Month+1) % 13];
      }

      /**********************************************************/
      // Method:  public string ToString ()
      // Purpose: Gets the string representation of this MonthFilter.
      //          E.g. ('6 - Jun'), ('9 - Sep'), or ('All').
      // Returns: The string representation of this MonthFilter.
      // Outputs: string
      /**********************************************************/
      public override string ToString()
      {
         return Text;
      }
   }
}