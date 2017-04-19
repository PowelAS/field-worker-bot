using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Powel.BotClient.Infrastructure.Controls
{
    public abstract class BindableStackLayout<TView, TBinding> : StackLayout
        where TView : View, new()
        where TBinding : IEquatable<TBinding>
    {
        protected BindableStackLayout()
        {
            PropertyChanged += BindableStackLayout_PropertyChanged;
        }

        public bool AppentToEnd { get; set; }

        public virtual void PostAction(IList<View> reports)
        {
        }

        protected void SetList(IList<TBinding> reportCollection)
        {
            Device.BeginInvokeOnMainThread(
                () =>
                {
                    foreach (var report in reportCollection)
                    {
                        Children.Add(
                            new TView
                            {
                                BindingContext = report,
                                VerticalOptions = LayoutOptions.End
                            });
                    }

                    PostAction();
                });
        }

        private void BindableStackLayout_PropertyChanged(
            object sender,
            System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BindingContext")
            {
                if (!(BindingContext is IList<TBinding>) || !(BindingContext is INotifyCollectionChanged))
                {
                    throw new InvalidOperationException(
                        "Binding context does not implement required interface for Bindable stack layout");
                }

                var reportCollection = (IList<TBinding>)BindingContext;
                Children.Clear();

                var reportCollectionNotifier = BindingContext as INotifyCollectionChanged;
                reportCollectionNotifier.CollectionChanged += BindingContext_CollectionChanged;

                if (reportCollection.Count == 0)
                {
                    return;
                }

                SetList(reportCollection);
            }
        }

        private void BindingContext_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newItem in e.NewItems)
                {
                    var item = new TView
                    {
                        BindingContext = (TBinding)newItem
                    };
                    if (!AppentToEnd)
                    {
                        Children.Insert(0, item);
                    }
                    else
                    {
                        Children.Add(item);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                Children.RemoveAt(e.OldStartingIndex);
                Children.Insert(
                    e.OldStartingIndex,
                    new TView
                    {
                        BindingContext = e.NewItems[0]
                    });
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var itemToDelete in e.OldItems)
                {
                    var first = Children.First(x => x.BindingContext.Equals(itemToDelete));

                    Children.Remove(first);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Children.Clear();
            }
            else
            {
                throw new NotImplementedException();
            }

            PostAction();
        }

        private void PostAction()
        {
            PostAction(Children);
        }
    }
}