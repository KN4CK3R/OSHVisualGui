﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonPrettyPrinterPlus.JsonPrettyPrinterInternals.JsonPPStrategies
{
    public class ColonCharacterStrategy : ICharacterStrategy
    {
        public void ExecutePrintyPrint(JsonPPStrategyContext context)
        {
            if (context.IsProcessingString)
            {
                context.AppendCurrentChar();
                return;
            }

            context.IsProcessingVariableAssignment = true;
            context.AppendCurrentChar();
            context.AppendSpace();
        }

        public char ForWhichCharacter
        {
            get { return ':'; }
        }
    }
}
