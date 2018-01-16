using Fusee.Math.Core;
using System;
public class Vector3D {
    
    public double x;
    public double y;
    public double z;

    public Vector3D(double x, double y, double z) {

        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3D(double t, double p) {

        this.x = System.Math.Cos(t) * System.Math.Sin(p);
        this.y = System.Math.Cos(t) * System.Math.Sin(p);
        this.z = System.Math.Sin(t);
    }

    public double length() {

        return System.Math.Sqrt(x*x + y*y + z*z);
    }

    public void normalize() {

        this.x = x/this.length();
        this.y = y/this.length();
        this.z = z/this.length();

        return;
    }

    public double dotproduct(Vector3D v) {

        return (this.x * v.x + this.y * v.y + this.z * this.z);
    }

    public void crossproduct(Vector3D v) {

        Vector3D uxv = new Vector3D(0,0,0);

        uxv.x = (this.y * v.z) - (this.z * v.y);
        uxv.y = (this.z * v.x) - (this.z * v.x);
        uxv.z = (this.x * v.y) - (this.y * v.x);

        return;
    }

    public double angle(Vector3D v) {

    return System.Math.Acos(this.dotproduct(v)/(this.length() * v.length()));
    }

    public void rotation(double g, Vector3D drehachse) {
    
        this.x = (
                  (System.Math.Pow(drehachse.x, 2) + System.Math.Cos(g) * (1 - System.Math.Pow(drehachse.x, 2))) +
                  (drehachse.x * drehachse.y * (1 - System.Math.Cos(g)) + drehachse.z * System.Math.Sin(g)) +
                  (drehachse.x * drehachse.z * (1 - System.Math.Cos(g)) - drehachse.y * System.Math.Sin(g))
                 );

        this.y = (
                  (drehachse.x * drehachse.y * (1 - System.Math.Cos(g)) - drehachse.z * System.Math.Sin(g)) +
                  (System.Math.Pow(drehachse.y, 2) + System.Math.Cos(g) * (1 - System.Math.Pow(drehachse.y, 2))) +
                  (drehachse.y * drehachse.z * (1 - System.Math.Cos(g)) + drehachse.x * System.Math.Sin(g))

                 );
        this.z = (
                  (drehachse.x * drehachse.z * (1 - System.Math.Cos(g)) + drehachse.y * System.Math.Sin(g)) +
                  (drehachse.y * drehachse.z * (1 - System.Math.Cos(g)) - drehachse.x * System.Math.Sin(g)) +
                  (System.Math.Pow(drehachse.z, 2) + System.Math.Cos(g) * (1 - System.Math.Pow(drehachse.z, 2)))
                 );

        return;
    }
}