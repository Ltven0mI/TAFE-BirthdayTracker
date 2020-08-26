using System.Collections.Generic;
using System;

namespace BirthdayTracker
{
   /**********************************************************/
   // Filename:   MonthFilter.cs
   // Purpose:    Provide a centralized class for use in
   //             filtering based on months of the year.
   // Author:     Wade Rauschenbach
   // Version:    1.0.0
   // Date:       26-Aug-2020
   // Tests:      Unit test completed 25-Aug-2020
   /**********************************************************/

   /*********************** Changelog ************************/
   // [Unreleased]
   // 
   // [1.0.0] - 26-Aug-2020
   // | [Added]
   // | - Add XML documentation comments for public API.
   // 
   // [0.1.0] 20-Aug-2020
   // | [Added]
   // | - Initial MonthFilter implementation.
   /**********************************************************/
   
   /// <summary>
   /// Provides a centralized class for use in filtering based on
   /// months of the year.
   /// </summary>
   /// <remarks>
   /// This class contains static filter instances for JAN through to DEC
   /// as well as an initial ALL filter.
   /// </remarks>
   public class MonthFilter
   {
      private static List<MonthFilter> filterList = new List<MonthFilter>();
      
      /// <summary>An all inclusive <see cref="MonthFilter"/>.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'All'</c>.</value>
      public static readonly MonthFilter All = new MonthFilter("All");
      
      /// <summary>A <see cref="MonthFilter"/> for the month of January.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'1 - Jan'</c>.</value>
      public static readonly MonthFilter Jan = new MonthFilter("1 - Jan");
      
      /// <summary>A <see cref="MonthFilter"/> for the month of February.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'2 - Feb'</c>.</value>
      public static readonly MonthFilter Feb = new MonthFilter("2 - Feb");
      
      /// <summary>A <see cref="MonthFilter"/> for the month of March.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'3 - Mar'</c>.</value>
      public static readonly MonthFilter Mar = new MonthFilter("3 - Mar");
      
      /// <summary>A <see cref="MonthFilter"/> for the month of April.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'4 - Apr'</c>.</value>
      public static readonly MonthFilter Apr = new MonthFilter("4 - Apr");

      /// <summary>A <see cref="MonthFilter"/> for the month of May.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'5 - May'</c>.</value>
      public static readonly MonthFilter May = new MonthFilter("5 - May");

      /// <summary>A <see cref="MonthFilter"/> for the month of June.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'6 - Jun'</c>.</value>
      public static readonly MonthFilter Jun = new MonthFilter("6 - Jun");

      /// <summary>A <see cref="MonthFilter"/> for the month of July.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'7 - Jul'</c>.</value>
      public static readonly MonthFilter Jul = new MonthFilter("7 - Jul");

      /// <summary>A <see cref="MonthFilter"/> for the month of August.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'8 - Aug'</c>.</value>
      public static readonly MonthFilter Aug = new MonthFilter("8 - Aug");

      /// <summary>A <see cref="MonthFilter"/> for the month of September.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'9 - Sep'</c>.</value>
      public static readonly MonthFilter Sep = new MonthFilter("9 - Sep");

      /// <summary>A <see cref="MonthFilter"/> for the month of October.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'10 - Oct'</c>.</value>
      public static readonly MonthFilter Oct = new MonthFilter("10 - Oct");

      /// <summary>A <see cref="MonthFilter"/> for the month of November.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'11 - Nov'</c>.</value>
      public static readonly MonthFilter Nov = new MonthFilter("11 - Nov");

      /// <summary>A <see cref="MonthFilter"/> for the month of December.</summary>
      /// <value>A <see cref="MonthFilter"/> represented as <c>'12 - Dec'</c>.</value>
      public static readonly MonthFilter Dec = new MonthFilter("12 - Dec");

      /// <summary>
      /// The <c>string</c> representation of the <see cref="MonthFilter"/>.
      /// </summary>
      /// <value>
      /// <c>"All"</c> for <see cref="MonthFilter.All"/>,
      /// <c>"1 - Jan"</c> for <see cref="MonthFilter.Jan"/>, ... ,
      /// <c>"12 - Dec"</c> for <see cref="MonthFilter.Dec"/>.
      /// </value>
      public string Text { get; private set; }

      /// <summary>
      /// The <c>index</c> of the <see cref="MonthFilter"/>.
      /// </summary>
      /// <value>
      /// <c>0</c> for <see cref="MonthFilter.All"/> and
      /// <c>1</c> through to <c>12</c> for months
      /// <see cref="MonthFilter.Jan"/> through to <see cref="MonthFilter.Dec"/>.
      /// </value>
      public int Month { get; private set; }

      /**********************************************************/
      // Method:  private MonthFilter (string text)
      // Purpose: Creates a new instance of MonthFilter with the
      //          'Text' property set to the passed 'text' parameter.
      // Inputs:  string text
      /**********************************************************/
      
      /// <summary>
      /// Creates a new instance of <see cref="MonthFilter"/> with
      /// <see cref="MonthFilter.Text"/> set to the passed <paramref name="text"/> parameter.
      /// </summary>
      /// <param name="text">The text representation of the filter.</param>
      private MonthFilter(string text)
      {
         Text = text;
         Month = filterList.Count;
         filterList.Add(this);
      }

      /**********************************************************/
      // Method:  public MonthFilter ()
      // Purpose: Gets the next MonthFilter after this.
      //          E.g. from ('8 - Aug') to ('9 - Sep') or
      //          from ('12 - Dec') to ('All').
      // Returns: The next MonthFilter based on the 'Month'.
      // Outputs: MonthFilter
      /**********************************************************/
      
      /// <summary>
      /// Gets the next <see cref="MonthFilter"/> after this.
      /// <para>
      /// E.g. from <see cref="MonthFilter.Aug"/> to <see cref="MonthFilter.Sep"/>
      /// or from <see cref="MonthFilter.Dec"/> to <see cref="MonthFilter.All"/>.
      /// </para>
      /// </summary>
      /// <returns>
      /// The next <see cref="MonthFilter"/> based on the <see cref="MonthFilter.Month"/>
      /// </returns>
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
      
      /// <summary>
      /// Gets the <c>string</c> representation of this <see cref="MonthFilter"/>.
      /// <para>E.g. <c>'6 - Jun'</c>, <c>'9 - Sep'</c>, or <c>'All'</c>.</para>
      /// </summary>
      /// <returns>
      /// The <c>string</c> representation of this <see cref="MonthFilter"/>.
      /// </returns>
      public override string ToString()
      {
         return Text;
      }
   }
}