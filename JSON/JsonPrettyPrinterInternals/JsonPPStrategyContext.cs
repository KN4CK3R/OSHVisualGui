﻿using System;
using System.Collections.Generic;
using System.Text;
using JsonPrettyPrinterPlus.JsonPrettyPrinterInternals.JsonPPStrategies;

namespace JsonPrettyPrinterPlus.JsonPrettyPrinterInternals
{
    public class JsonPPStrategyContext
    {

        private const string Space = " ";
        public int SpacesPerIndent = 4;

        private string _indent = string.Empty;
        public string Indent
        {
            get
            {
                if (SpacesPerIndent == 0)
                    return string.Empty;

                if (_indent == string.Empty)
                    InitializeIndent();

                return _indent;
            }
        }

        private void InitializeIndent()
        {
            for (int iii = 0; iii < SpacesPerIndent; iii++)
                _indent += Space;
        }

        private readonly PPScopeState _scopeState = new PPScopeState();

        public bool IsInArrayScope
        {
            get
            {
                return _scopeState.IsTopTypeArray;
            }
        }

        private void AppendIndents(int indents)
        {
            for (var iii = 0; iii < indents; iii++)
                _outputBuilder.Append(Indent);
        }

        public bool IsProcessingVariableAssignment;
        private char _previousChar;
        public bool IsProcessingDoubleQuoteInitiatedString { get; set; }
        public bool IsProcessingSingleQuoteInitiatedString { get; set; }

        public bool IsProcessingString
        {
            get
            {
                return IsProcessingDoubleQuoteInitiatedString || IsProcessingSingleQuoteInitiatedString;
            }
        }

        public bool IsStart
        {
            get
            {
                return _outputBuilder.Length == 0;
            }
        }

        public bool WasLastCharacterABackSlash
        {
            get
            {
                return _previousChar == '\\';
            }
        }

        private readonly IDictionary<char, ICharacterStrategy> _strategyCatalog = new Dictionary<char, ICharacterStrategy>();

        private StringBuilder _outputBuilder;
        private char _currentCharacter;

        public void PrettyPrintCharacter(char curChar, StringBuilder output)
        {
            _currentCharacter = curChar;

            var strategy = _strategyCatalog.ContainsKey(curChar)
                                ? _strategyCatalog[curChar]
                                : new DefaultCharacterStrategy();

            _outputBuilder = output;

            strategy.ExecutePrintyPrint(this);

            _previousChar = curChar;
        }

        public void AppendCurrentChar()
        {
            _outputBuilder.Append(_currentCharacter);
        }

        public void AppendNewLine()
        {
            _outputBuilder.Append(Environment.NewLine);
        }

        public void BuildContextIndents()
        {
            AppendNewLine();
            AppendIndents(_scopeState.ScopeDepth);
        }

        public void EnterObjectScope()
        {
            _scopeState.PushObjectContextOntoStack();
        }

        public void CloseCurrentScope()
        {
            _scopeState.PopJsonType();
        }

        public void EnterArrayScope()
        {
            _scopeState.PushJsonArrayType();
        }

        public void AppendSpace()
        {
            _outputBuilder.Append(Space);
        }

        public void ClearStrategies()
        {
            _strategyCatalog.Clear();
        }

        public void AddCharacterStrategy(ICharacterStrategy strategy)
        {
            _strategyCatalog[strategy.ForWhichCharacter] = strategy;
        }
    }
}