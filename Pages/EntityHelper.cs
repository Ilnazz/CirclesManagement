using System;

namespace CirclesManagement.Pages
{
    public class EntityHelper
    {
        public Predicate<object> IsDeleted; // помечена ли сущность как удалённая

        public Func<object> Builder; // создаёт новую пустую сущность (со значенями полей по умолчанию)

        public Predicate<object> IsBlank; // является ли сущность пустой (только что созданной)

        public Func<object, object, (bool, string)> Comparer; // являются ли поля сущностей одинаковыми
        
        public Func<object, (bool, string)> Deleter; // помечает сущность как удалённую и возвращает (истину, ""),
                                                     // или возвращает (ложь, "причина по которой нельзя удалить сущность")

        public Func<object, (bool, string)> Validator; // проверяет поля сущности на корректность заполнения; работает как Deleter

        public Func<object, string, bool> SearchTextMatcher; // удовлетворяет ли сущность стркое поиска

        public Action<object> SavePreparator; // подготавливает сущность к сохранению в базе данных
    }
}