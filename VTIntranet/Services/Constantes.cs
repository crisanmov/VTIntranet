using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Services
{
    public class Constantes
    {
        public static bool activeRecord = true;
        public static bool inActiveRecord = true;
        public static bool completedProcess = true;
        public static int MaxQuickDescriptionCharacters = 50;

        public enum DescriptionVTI
        {
            MaxQuickDescriptionCharacters = 250,
            MaxQuickDescriptionFileCharacters = 120,
            QuickDescriptionCharacters = 128,
            MaxQuickDescriptionCharactersTooltip = 50
        };
    }
}