using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Galaxy.Core.Collision;
using System.Diagnostics;
using System.Windows;
using Galaxy.Core.Actors;
using Galaxy.Core.Environment;
using System.Timers;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Galaxy.Environments.Actors;



namespace Galaxy.Environments.Actors
{
  public class Ship : DethAnimationActor
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

    public Ship(ILevelInfo info):base(info)
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
    public EnemyBullet CreateEnemyBullet(Ship ship)
    {
        var enbullet = new EnemyBullet(Info);
        int positionY = ship.Position.Y;
        int positionX = ship.Position.X;
        enbullet.Position = new Point(positionX, positionY);
        enbullet.Load();
        return enbullet;
    }
    public override void Load()
    {
      Load(@"Assets\ship.png");
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
        
        Position = new Point((int)(Position.X), (int)(Position.Y + Position.X / 70));
        }

    #endregion
  }
}
