using System;
using Autofac.Features.OwnedInstances;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain.Repositories;

namespace MingleTransitionMonitor.Service
{
    public class Runner
    {
        private readonly IMingleProjectRepository _mingleProjectRepository;
        private readonly Func<Owned<NotificationController>> _scopedControllerFactory;
        
        public Runner(IMingleProjectRepository mingleProjectRepository, Func<Owned<NotificationController>> scopedControllerFactory )
        {
            _mingleProjectRepository = mingleProjectRepository;
            _scopedControllerFactory = scopedControllerFactory;
        }

        public void RunForAllMingleProjects()
        { 
            foreach (var mingleProject in _mingleProjectRepository.GetAllTransitionsToMonitor())
            {
                string project = mingleProject.Name;

                LoggingWrapper.Debug<Runner>("Running project {0}", project);

                using (var controllerUnitOfWork = _scopedControllerFactory())
                {

                    var controller = controllerUnitOfWork.Value;
                    try
                    {
                        controller.Monitor(mingleProject);
                    }
                    catch (Exception e)
                    {
                        LoggingWrapper.Error<Runner>("Error while processing project {0}", e, project);
                    }
                }
            }

        }
    }
}