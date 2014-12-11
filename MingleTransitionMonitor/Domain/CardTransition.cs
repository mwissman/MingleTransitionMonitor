using System;
using System.Collections.Generic;

namespace MingleTransitionMonitor.Domain
{
    public class CardTransition
    {
        public CardTransition()
        {
        }

        public CardTransition( string transitionProperty, string cardType)
        {  
            TransitionProperty = transitionProperty;
            CardType = cardType;
        }

        public string TransitionProperty { get;  set; }
        public string CardType { get;  set; }


        public IEnumerable<Uri> TransitionWebHookCallbacks { get; set; } 
    }
}