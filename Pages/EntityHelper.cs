using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclesManagement.Pages
{
    public class EntityHelper
    {
        public Predicate<object> IsDeleted; // returns true if entity marked as deleted

        public Func<object> Builder; // creates and returns new blank entity

        public Predicate<object> IsBlank; // returns true if entity wasn't changed after creation

        public Func<object, (bool, string)> Deleter; // marks entity as deleted (if entity isn't tied with another entites)
                                                     // and returns (true, "") if entity
                                     // (don't removes it from source collection)

        public Func<object, (bool, string)> Validator; // returns (true, "") when success,
                                                       // and (false, "description msg") when error
        
        public Func<object, string, bool> SearchTextMatcher;

        // Название сущности
        public Word Title;

        // Падежи слова в ед. и мн. числах
        public struct Word
        {
            public WordCases Singular;
            public WordCases Plural;
        }

        // Падежи слова
        public struct WordCases
        {
            public string Nominative; // именительный
            public string Genitive; // родительный
            public string Dative; // дательный
            public string Accusative; // винительный
            public string Ablative; // творительный
            public string Prepositional; // предложный
        }
    }
}
