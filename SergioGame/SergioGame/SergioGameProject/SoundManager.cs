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

namespace SergioGameProject
{
    /// <summary>
    /// Class responsible for creating entities used in the game
    /// </summary>
    static class SoundManager
    {

        public static MusicInfo getRockBrakingMusic()
        {
            MusicInfo musicInfo = new MusicInfo("Content/Music/rock_breaking.wma");
            return musicInfo;
        }

        public static MusicInfo getLaserShotMusic()
        {
            MusicInfo musicInfo = new MusicInfo("Content/Music/laser.wma");
            return musicInfo;
        }

        public static SoundInfo getGameLoopSound() {
            return new SoundInfo("Content/Music/game loop.mp3");
        }
    }


}
