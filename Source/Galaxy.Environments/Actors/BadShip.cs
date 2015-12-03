using System;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Galaxy.Environments.Actors
{
    class BadShip : DethAnimationActor
    {
        #region Constant

        private const int MaxSpeed = 1;
        private const long StartFlyMs = 2000;

        #endregion

        #region Private fields

        private bool m_flying;
        private Stopwatch m_flyTimer;
        

        #endregion

        #region Constructors

        public BadShip(ILevelInfo info):base(info)
        {
            Width = 30;
            Height = 30;
            ActorType = ActorType.Enemy;
            
        }

        #endregion

        #region Overrides

        public override void Update()
        {
            base.Update();

            if (!IsAlive)
                return;

            if (!m_flying)
            {
                if (m_flyTimer.ElapsedMilliseconds <= StartFlyMs) return;

                m_flyTimer.Stop();
                m_flyTimer = null;
                h_changePosition();
                m_flying = true;
            }
            else
            {
                h_changePosition();
            }
        }

        #endregion

        #region Overrides

        public override void Load()
        {
            Load(@"Assets\badship.png");
            if (m_flyTimer == null)
            {
                m_flyTimer = new Stopwatch();
                m_flyTimer.Start();
            }
        }

        #endregion

        #region Private methods

        private void h_changePosition()
        {            
            Size levelSize = Info.GetLevelSize();

            int yNewPosition = (int)(Position.Y + Position.X / 100);

                if (Position.X < 150 || Position.X > levelSize.Width - 200)
                {
                    Position = new Point((int)(Position.X - MaxSpeed-1), yNewPosition + 1);
                }
                else
                {
                    Position = new Point((int)(Position.X - MaxSpeed), yNewPosition);
                }
            }
            
        }

        #endregion
    }

