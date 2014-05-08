using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveEngine.Framework;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
using WaveEngine.Components.Animation;
using WaveEngine.Components.UI;
using WaveEngine.Framework.Physics2D;
using WaveEngine.Common.Media;
using WaveEngine.Framework.Sound;
using SergioGameProject.assets;

namespace SergioGameProject
{
    /// <summary>
    /// Class responsible for managing the sound creation on the game
    /// </summary>
    static class SoundManager
    {
        private static SoundInfo rockBreaking = null;
        private static SoundInfo laserShot = null;

        public static SoundInfo getRockBrakingSound()
        {
            
            if (rockBreaking == null)
            {
                rockBreaking = new SoundInfo(PathManager.rockBreakingSound);
            }
            return rockBreaking;
        }

        public static SoundInfo getLaserShotSound()
        {
            if (laserShot == null)
            {
                laserShot = new SoundInfo(PathManager.laserSound);
            }
            return laserShot;
        }

        public static MusicInfo getGameLoopSound()
        {

            MusicInfo gameLoop = new MusicInfo(PathManager.gameLoppMusic);

            return gameLoop;

        }
    }


}
