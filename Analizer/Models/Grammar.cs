using System.Collections.Generic;
using System.Text.RegularExpressions;
using Analizer.Exception;

namespace Analizer.Models
{
    public class Grammar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rules">View: rightRule->leftRule</param>
        public Grammar(List<string> rules)
        {
            if (rules.Count == 0) // проверка на наличие правил
            {
                throw new EmptyRulesException();
            }
            var i = 0;
            foreach (string stringRule in rules)
            {
                i++;
                (string firstUnitRule, string secontUnitRule) rule = (stringRule.Substring(0, stringRule.IndexOf('-')), stringRule.Substring(stringRule.IndexOf('-') + 2)); //разделяем правило на левое и правое
                if (CheckAGrammar(rule))
                {
                    Type = Type < GrammarType.A ? GrammarType.A : Type ;
                    continue;
                }

                if (CheckContextFreeGrammar(rule))
                {
                    Type = Type < GrammarType.ContextFree ? GrammarType.ContextFree : Type;
                    continue;
                }

                if (rule.firstUnitRule == rule.secontUnitRule)
                {
                    throw new EqualsRulesException($"{i}");
                }

                if (CheckContextSensitiveGrammar(rule))
                {
                    Type = Type < GrammarType.ContextSensitive ? GrammarType.ContextSensitive : Type;
                    continue;
                }
                
                Type = GrammarType.Zero;
            }
        }

        public GrammarType Type { get; set; } = GrammarType.A;

        /// <summary>
        /// Проверить правило на А-грамматику
        /// </summary>
        /// <param name="rule">Правило</param>
        /// <returns>Правило</returns>
        private bool CheckAGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length == 1 || rule.secontUnitRule.Length <= 2)
            {
                // размер левой части в А гр всегда равен 1, а второй может быть как 1 так и 2
                if (new Regex(@"[A-Z]").Match(rule.firstUnitRule).Length == 0)
                {
                    // левая часть правила должна быть нетерминальный символом
                    return false;
                }

                if (rule.secontUnitRule.Length == 1)
                {
                    if (new Regex(@"[A-Z]").Match(rule.secontUnitRule).Length != 0)
                    {
                        // если в правой части один символ то он должен быть терминалом
                        return false;
                    }

                    return true; // если размер подходит и это терминал, то это A-гр
                }

                if (new Regex(@"[A-Z]").Matches(rule.secontUnitRule).Count == 1)
                {
                    //обязательно должен быть нетерминал если размер правой части равен 2
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Проверить правило на КС-грамматику
        /// </summary>
        /// <param name="rule">Правило</param>
        /// <returns>Правило</returns>
        private bool CheckContextFreeGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length == 1 && new Regex(@"[A-Z]").Match(rule.firstUnitRule).Length == 1)
            {
                // правая часть всегда равна 1, и там обязательно должен быть нетерминал
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверить правило на КЗ-граммматику
        /// </summary>
        /// <param name="rule">Правило</param>
        /// <returns>Правило</returns>
        private bool CheckContextSensitiveGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length > rule.secontUnitRule.Length) // правая часть должна быть больше или равна левой
            {
                return false;
            }

            while (rule.firstUnitRule.Length != 1)
            {
                rule = DeleteContext(rule); // обрезаем контекст у правила
            }

            return CheckContextFreeGrammar(rule); // должно выглядеть как правило КС граммматики
        }

        /// <summary>
        /// Удалить контекст по одному символу с каждой стороны если он есть
        /// </summary>
        /// <param name="rule">Правило</param>
        /// <returns>Правило</returns>
        private (string firstUnitRule, string secontUnitRule) DeleteContext((string firstUnitRule, string secontUnitRule) rule)
        {
            if(rule.firstUnitRule[0] == rule.secontUnitRule[0])
            {
                rule.firstUnitRule = rule.firstUnitRule.Substring(1); // обрезать первый символ
                rule.secontUnitRule = rule.secontUnitRule.Substring(1);
            }
            
            if (rule.firstUnitRule[rule.firstUnitRule.Length - 1] == rule.secontUnitRule[rule.secontUnitRule.Length - 1])
            {
                rule.firstUnitRule = rule.firstUnitRule.Substring(0, rule.firstUnitRule.Length - 1); // обрезать последний символ
                rule.secontUnitRule = rule.secontUnitRule.Substring(0, rule.secontUnitRule.Length - 1);
            }

            return rule;
        }

    }
}
