using System;

namespace ComLib.Lang.Tests.Common
{
    public class KlassNested1
    {
        public static double   KPropNumber { get; set;}
        public static string   KPropString { get; set;}
        public static bool     KPropBool   { get; set;}
        public static DateTime KPropDate   { get; set;}
        public static TimeSpan KPropTime   { get; set;}


        // Props
        public double   PropNumber { get; set; }
        public string   PropString { get; set; }
        public bool     PropBool   { get; set; }
        public DateTime PropDate   { get; set; }
        public TimeSpan PropTime   { get; set; }


        public void SetProps(double a, string b, bool c, DateTime d, TimeSpan e)
        {
            PropNumber = a;
            PropString = b;
            PropBool = c;
            PropDate = d;
            PropTime = e;
        }
    }


    public class Klass2
    {
        static Klass2()
        {
            Reset();
        }


        public static void Reset()
        {
            KPropNumber = 1234;
            KPropString = "Fluent";
            KPropBool   = true;
            KPropDate   = new DateTime(2012, 6, 1);
            KPropTime   = new TimeSpan(0, 8, 10, 35);
        }


        public static void SetClassProps(double a, string b, bool c, DateTime d, TimeSpan e, KlassNested1 f)
        {
            KPropNumber = a;
            KPropString = b;
            KPropBool   = c;
            KPropDate   = d;
            KPropTime   = e;
            KPropObject = f;
            KlassNested1.KPropNumber = f.PropNumber;
            KlassNested1.KPropString = f.PropString;
            KlassNested1.KPropBool = f.PropBool;
            KlassNested1.KPropDate = f.PropDate;
            KlassNested1.KPropTime = f.PropTime;

        }


        public Klass2()
        {
            ResetInstance();
        }


        public void ResetInstance()
        {
            Prop1 = 123;
            Prop2 = "fluent";
            Prop3 = true;
            Prop4 = new DateTime(2012, 6, 1);
        }


        // START STATIC 
        // ---------------------------------------------------------------------------------		
        private static double _kprop1;
        private static string _kprop2;
        private static bool _kprop3;
        private static DateTime _kprop4;
        private static TimeSpan _kprop5;
        private static KlassNested1 _kprop6;

        // Props
        public static double   KPropNumber     { get { return _kprop1; } set { _kprop1 = value; } }
        public static string   KPropString     { get { return _kprop2; } set { _kprop2 = value; } }
        public static bool     KPropBool       { get { return _kprop3; } set { _kprop3 = value; } }
        public static DateTime KPropDate       { get { return _kprop4; } set { _kprop4 = value; } }
        public static TimeSpan KPropTime       { get { return _kprop5; } set { _kprop5 = value; } }
        public static KlassNested1 KPropObject { get { return _kprop6; } set { _kprop6 = value; } }


        // Methods
        public static double   KMethodRetNumber(double a, string b, bool c, DateTime d, TimeSpan e) { return a; }
        public static string   KMethodRetString(double a, string b, bool c, DateTime d, TimeSpan e) { return b; }
        public static bool     KMethodRetBool(  double a, string b, bool c, DateTime d, TimeSpan e) { return c; }
        public static DateTime KMethodRetDate(  double a, string b, bool c, DateTime d, TimeSpan e) { return d; }
        public static TimeSpan KMethodRetTime(  double a, string b, bool c, DateTime d, TimeSpan e) { return e; }
        // END STATIC 
        // ----------------------------------------------------------------------------------



        // START INSTANCE
        // ---------------------------------------------------------------------------------		
        private double _prop1;
        private string _prop2;
        private bool _prop3;
        private DateTime _prop4;
        private TimeSpan _prop5;


        // Props
        public double   Prop1 { get { return _prop1; } set { _prop1 = value; } }
        public string   Prop2 { get { return _prop2; } set { _prop2 = value; } }
        public bool     Prop3 { get { return _prop3; } set { _prop3 = value; } }
        public DateTime Prop4 { get { return _prop4; } set { _prop4 = value; } }
        public TimeSpan Prop5 { get { return _prop5; } set { _prop5 = value; } }


        // Methods
        public double   Method1(double a, string b, bool c, DateTime d) { return a; }
        public string   Method2(double a, string b, bool c, DateTime d) { return b; }
        public bool     Method3(double a, string b, bool c, DateTime d) { return c; }
        public DateTime Method4(double a, string b, bool c, DateTime d) { return d; }
        public TimeSpan Method5(double a, string b, bool c, DateTime d, TimeSpan e) { return e; }

        // END INSTANCE 
        // ----------------------------------------------------------------------------------		
    }
}
