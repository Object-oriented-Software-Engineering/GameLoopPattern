using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLoopPattern {

    public class CCartesianPoint {
        public double X { get; set; }  // double gives us more precision than int
        // in case of minor movements
        public double Y { get; set; }
    }

    public class CGameObject {
        public CCartesianPoint _position = new CCartesianPoint() { X = 0, Y = 0 };
        private double _mass = 10;
        private double _velocityX = 150;
        private double _velocityY = 0;
        private double _accelerationX;
        private double _accelerationY;


        private List<object> m_aspects;

        public double Mass {
            get { return _mass; }
            set {
                if (value > 0) {
                    _mass = value;
                }
            }
        }
        public CCartesianPoint Position {
            get { return _position; }
            set { _position = value; }
        }
        public double VelocityX {
            get { return _velocityX; }
            set { _velocityX = value; }
        }
        public double VelocityY {
            get { return _velocityY; }
            set { _velocityY = value; }
        }
        public double AccelerationX {
            get { return _accelerationX; }
            set { _accelerationX = value; }
        }

        public CGameObject() {
        }

        public void ApplyForceX(double force) {
            _accelerationX = force / _mass;
        }

        public void ApplyForceY(double force) {
            _accelerationY = force / _mass;
        }

        public void AddAspect(object aspect) {
            m_aspects.Add(aspect);
        }

        public T GetAspect<T>() {
            foreach (var aspect in m_aspects) {
                if (aspect is T) {
                    return (T)aspect;
                }
            }
            return default(T);
        }

        public void Update(object sender, TimeElapsedEventArgs dt) {
            // Euler integration
            _velocityX += _accelerationX * dt.TimeElapsed;
            _position.X += (_velocityX * dt.TimeElapsed);
            _velocityY += _accelerationY * dt.TimeElapsed;
            _position.Y += (_velocityY * dt.TimeElapsed);

            Trace.WriteLine("");
            Trace.WriteLine("TimeElapsed: " + dt.TimeElapsed);
            Trace.WriteLine("X: " + _position.X + " Y: " + _position.Y);
            Trace.WriteLine("VelocityX: " + _velocityX + " VelocityY: " + _velocityY);

        }

    }
}
