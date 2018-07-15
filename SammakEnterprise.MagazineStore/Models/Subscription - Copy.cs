using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SammakEnterprise.MagazineStore.Models
{
    public class CategoryMagazine
    {
        public string Category { get; set; }
        public List<Magazine> Magazines { get; set; }
        public int MagazineCount { get; set; }
    }

    public class Subscription
    {
        public string SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CategoryMagazine> CategoryMagazines { get; set; }
    }

    public class Subscriptions : List<Subscription>
    {
        protected readonly List<CategoryMagazine> _categoryMagazines = new List<CategoryMagazine>();

        public Subscriptions(string subscriberId, string subscriberName)
        {
            SubscriberId = subscriberId;
            SubscriberName = subscriberName;
        }

        public string SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public List<CategoryMagazine> CategoryMagazines
        {
            get { return new List<CategoryMagazine>(_categoryMagazines); }
        }

        public void AddCategoryMagazine(CategoryMagazine categoryMagazine)
        {
            if (categoryMagazine != null)
            {
                if (_categoryMagazines.Find(c => c.Category == categoryMagazine.Category) == null)
                {
                    _categoryMagazines.Add(categoryMagazine);
                }
            }
        }

    }
}
