using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CustomTreeWizard.Categories
{
    public class CategoryItem : PageItem
    {
        public static DependencyProperty CategoryPagesProperty = DependencyProperty.Register(
            "WizardCategoriesPages",
            typeof (ObservableCollection<PageItem>),
            typeof(CategoryItem),
            new FrameworkPropertyMetadata(new ObservableCollection<PageItem>(), OnWizardPagesValueChanged));

        public CategoryItem()
        {
            WizardCategoriesPages = new ObservableCollection<PageItem>();
        }

        public ObservableCollection<PageItem> WizardCategoriesPages
        {
            get { return GetValue(CategoryPagesProperty) as ObservableCollection<PageItem>; }
            set { SetValue(CategoryPagesProperty, value); }
        }

        public ObservableCollection<object> ParentCollection { get; set; }

        private static void OnWizardPagesValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CategoryItem;
            if (control == null)
            {
                return;
            }

            control.WizardCategoriesPages = (ObservableCollection<PageItem>)e.NewValue;
            if (control.WizardCategoriesPages == null)
            {
                return;
            }

            var oldValue = e.OldValue as ObservableCollection<PageItem>;
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= control.WizardPagesOnCollectionChanged;
            }

            control.WizardCategoriesPages.CollectionChanged += control.WizardPagesOnCollectionChanged;
        }

        private void WizardPagesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (notifyCollectionChangedEventArgs == null)
            {
                return;
            }

            if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add )
            {
                if (notifyCollectionChangedEventArgs.NewItems != null)
                {
                    foreach (PageItem newItem in notifyCollectionChangedEventArgs.NewItems)
                    {
                        newItem.ParentPageItem = this;
                        newItem.ContentChanged += OnItemContentChanged;
                    }
                }
            }
        }

        private void OnItemContentChanged(object sender, ContentChangedEventArgs args)
        {
            if (ContentChanged != null)
            {
                ContentChanged(this, args);
            }
        }
    }
}
