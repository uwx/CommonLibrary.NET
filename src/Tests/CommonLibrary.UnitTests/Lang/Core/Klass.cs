using System;

namespace ComLib.Lang.Tests.Common
{
    public class Klass
    {
        static Klass()
        {
            Reset();
        }


        public static void Reset()
        {
            KProp1 = 1234;
            KProp2 = "Fluent";
            KProp3 = true;
            KProp4 = new DateTime(2012, 6, 1);
        }


        public static void SetClassProps(double a, string b, bool c, DateTime d, TimeSpan e)
        {
            KProp1 = a;
            KProp2 = b;
            KProp3 = c;
            KProp4 = d;
            KProp5 = e;
        }


        public Klass()
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


        // Props
        public static double   KProp1 { get { return _kprop1; } set { _kprop1 = value; } }
        public static string   KProp2 { get { return _kprop2; } set { _kprop2 = value; } }
        public static bool     KProp3 { get { return _kprop3; } set { _kprop3 = value; } }
        public static DateTime KProp4 { get { return _kprop4; } set { _kprop4 = value; } }
        public static TimeSpan KProp5 { get { return _kprop5; } set { _kprop5 = value; } }


        // Methods
        public static double   KMethod1(double a, string b, bool c, DateTime d) { return a; }
        public static string   KMethod2(double a, string b, bool c, DateTime d) { return b; }
        public static bool     KMethod3(double a, string b, bool c, DateTime d) { return c; }
        public static DateTime KMethod4(double a, string b, bool c, DateTime d) { return d; }
        public static TimeSpan KMethod5(double a, string b, bool c, DateTime d, TimeSpan e) { return e; }
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
