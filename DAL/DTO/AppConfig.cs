using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCore3Sample
{
    [System.Serializable]
    public sealed class AppConfig
    {
        #region Properties

        public int version { get; set; }

        public string ConfigKey { get; set; } = AppConst.DEFAULT_STRING;

        public string ConfigValue { get; set; } = AppConst.DEFAULT_STRING;
        public string Rmrk { get; set; } = AppConst.DEFAULT_STRING;
        public string IsActive { get; set; } = AppConst.DEFAULT_STRING;
        public string CreID { get; set; } = AppConst.DEFAULT_STRING;
        public DateTime CreTime { get; set; } = AppConst.DEFAULT_DATE;
        public string ModID { get; set; } = AppConst.DEFAULT_STRING;
        public DateTime ModTime { get; set; } = AppConst.DEFAULT_DATE;

        #endregion
    }
    [System.Serializable]
    public sealed class AppConfigSO
    {
        public bool MatchAllFields { get; set; }


        public int[] versionArr { get; set; }
        public int version1 { get; set; }
        public int version2 { get; set; }
        public string[] ConfigKeyArr { get; set; }
        public string ConfigKey { get; set; }
        public string[] ConfigValueArr { get; set; }
        public string ConfigValue { get; set; }
        public string[] RmrkArr { get; set; }
        public string Rmrk { get; set; }
        public string[] IsActiveArr { get; set; }
        public string IsActive { get; set; }
        public string[] CreIDArr { get; set; }
        public string CreID { get; set; }
        public DateTime CreTime1 { get; set; }
        public DateTime CreTime2 { get; set; }
        public string[] ModIDArr { get; set; }
        public string ModID { get; set; }
        public DateTime ModTime1 { get; set; }
        public DateTime ModTime2 { get; set; }
    }
}
