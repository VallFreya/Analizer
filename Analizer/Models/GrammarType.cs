using System.ComponentModel.DataAnnotations;

namespace Analizer.Models
{
    public enum GrammarType
    {
        [Display(Name = "А-грамматика")]
        A,
        [Display(Name = "Контекстно-свободная грамматика")]
        ContextFree,
        [Display(Name = "Контекстно-зависимая грамматика")]
        ContextSensitive,
        [Display(Name = "Нулевая грамматика")]
        Zero
    }
}
