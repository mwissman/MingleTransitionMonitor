using System.Collections;
using System.Collections.Generic;
using MingleTransitionMonitor.Domain;
using ThoughtWorksMingleLib;

namespace MingleTransitionMonitor.Infrastructure
{
    public interface IMingleCardPropertyChangedBuilder
    {
        IList<MingleCardPropertyChanged> BuildFrom(MingleEventsFeedEntry entry);
    }
}