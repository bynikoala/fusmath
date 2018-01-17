using System;

using Fusee.Math;
using Fusee.Math.Core;
using static System.Math;

namespace FuseeApp
{   
    public class Grosskreis {
        static public float3 getPolCoord(float3 punkt) {

            double x = (double)punkt.x;
            double y = (double)punkt.y;
            double z = (double)punkt.z;

            double r        = Sqrt((x*x)+(y*y)+(z*z));
            double theta    = Acos(z/r);
            double phi      = Atan2(y,x);            

            return new float3((float)r, (float)theta, (float)phi);
        }
        static public float3 getKartCoord(float3 punkt) {
            double r        = (double)punkt.x;
            double theta    = (double)punkt.y;
            double phi      = (double)punkt.z;


            double x = r * Sin(theta)*Cos(phi);
            double y = r * Sin(theta)*Sin(phi);
            double z = r * Cos(theta);
            
            return new float3((float)x, (float)y, (float)z);
        }


    }
}