using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvmStdLib
{
    public static class Time
    {
        #region Ticks
        /// <summary>
        /// Relative ms counts, since start of app
        /// </summary>
        public static UInt32 Ticks
        {
            get {
                UpdateCountTicks();
                return (UInt32)countTicks;
            }
        }
        /// <summary>
        /// Relative ms counts, since start of app
        /// </summary>
        public static UInt64 Ticks64
        {
            get
            {
                UpdateCountTicks();
                return (UInt64)countTicks;
            }
        }
        private static long utcOldTicks = System.DateTime.UtcNow.Ticks;
        private static long countTicks;
        private static object tickLocker = new object();
        private static void UpdateCountTicks()
        {
            lock (tickLocker)
            {
                countTicks += (System.DateTime.UtcNow.Ticks - utcOldTicks) / 10000;
                utcOldTicks = System.DateTime.UtcNow.Ticks;
            }

        }
        #endregion
    }
}
