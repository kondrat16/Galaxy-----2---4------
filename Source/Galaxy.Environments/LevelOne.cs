#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Galaxy.Core.Actors;
using Galaxy.Core.Collision;
using Galaxy.Core.Environment;
using Galaxy.Environments.Actors;

#endregion

namespace Galaxy.Environments
{
  /// <summary>
  ///   The level class for Open Mario.  This will be the first level that the player interacts with.
  /// </summary>
  public class LevelOne : BaseLevel
  {
    private int m_frameCount;

    #region Constructors

    /// <summary>
    ///   Initializes a new instance of the <see cref="LevelOne" /> class.
    /// </summary>
    public LevelOne()
    {
        // Backgrounds
        FileName = @"Assets\LevelOne.png";

        // Enemies
        for (int i = 0; i < 5; i++)
        {
            var ship = new Ship(this);
            int positionY = ship.Height + 10;
            int positionX = 150 + i * (ship.Width + 50);

            ship.Position = new Point(positionX, positionY);

            Actors.Add(ship);
        }

        for (int i = 0; i < 1; i++)
        {
            var Lightning = new Lightning(this);
            int positionY = Lightning.Height + 200;
            int positionX = 200 + i * (Lightning.Width + 200);
            Lightning.Position = new Point(positionX, positionY);
            Actors.Add(Lightning);
        }

        for (int i = 0; i < 5; i++)
        {
            var badShip = new BadShip(this);
            int positionY = badShip.Height + 70;
            int positionX = 160 + i * (badShip.Width + 40);

            badShip.Position = new Point(positionX, positionY);

            Actors.Add(badShip);
        }

        // Player            
        Player = new PlayerShip(this);
        int playerPositionX = Size.Width / 2 - Player.Width / 2;
        int playerPositionY = Size.Height - Player.Height - 50;
        Player.Position = new Point(playerPositionX, playerPositionY);
        Actors.Add(Player);
    }
     
      

    #endregion

    #region Overrides

    private void h_dispatchKey()
    {
      if (!IsPressed(VirtualKeyStates.Space)) return;

      if(m_frameCount % 10 != 0) return;

      Bullet bullet = new Bullet(this)
      {
        Position = Player.Position
      };

      bullet.Load();
      Actors.Add(bullet);
    }

    public override BaseLevel NextLevel()
    {
      return new StartScreen();
    }
   

    public void GenerateBullet()
    {
        var datetime = DateTime.Now.Millisecond;
        Ship[] arShip = Actors.Where(actor => actor is Ship).Cast<Ship>().ToArray();
        var n = datetime;
        if (Convert.ToInt32(n) % 27 == 0 & Convert.ToInt32(n) > 0)
        {
            foreach (Ship ship in arShip)
            {
                {
                    Actors.Add(ship.CreateEnemyBullet(ship));
                }
            }
        }
    }

    public void DestroyBullet()
    {
        EnemyBullet[] arShip = Actors.Where(actor => actor is EnemyBullet).Cast<EnemyBullet>().ToArray();
        foreach (EnemyBullet enemy in arShip)
        {
            if (enemy.Position.Y >= DefaultHeight)
                Actors.Remove(enemy);
        }
    }

    public override void Update()
    {
      m_frameCount++;
      h_dispatchKey();

      base.Update();
      GenerateBullet();
      DestroyBullet();
     

      IEnumerable<BaseActor> killedActors = CollisionChecher.GetAllCollisions(Actors);

      foreach (BaseActor killedActor in killedActors)
      {
          if (killedActor.IsAlive & killedActor.ActorType != ActorType.Lightning)
          killedActor.IsAlive = false;
      }

      List<BaseActor> toRemove = Actors.Where(actor => actor.CanDrop).ToList();
      BaseActor[] actors = new BaseActor[toRemove.Count()];
      toRemove.CopyTo(actors);

      foreach (BaseActor actor in actors.Where(actor => actor.CanDrop))
      {
        Actors.Remove(actor);
      }

      if (Player.CanDrop)
        Failed = true;

      //has no enemy
      if (Actors.All(actor => actor.ActorType != ActorType.Enemy))
        Success = true;
    }

    #endregion
  }
}
