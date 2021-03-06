[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Academic.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Academic.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Academic.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using AcademicXXI.Data;
    using AcademicXXI.Repository.StudentRepository;
    using AcademicXXI.Services.StudentService;
    using AcademicXXI.Repository.StudyPlanRepository;
    using AcademicXXI.Services.StudyPlanService;
    using AcademicXXI.Repository.SubjectRepository;
    using AcademicXXI.Services.SubjectService;
    using AcademicXXI.Repository.SemesterRepository;
    using AcademicXXI.Services.SemesterService;
    using AcademicXXI.Repository.RecordRepository;
    using AcademicXXI.Services.RecordService;
    using AcademicXXI.Repository.ProfessorRepository;
    using AcademicXXI.Services.ProfessorService;
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Register DataContext
            kernel.Bind<AcademicXXIDataContext>().ToSelf().InRequestScope();

            //Register Repositories
            kernel.Bind<IStudentRepository>().To<StudentRepository>();
            kernel.Bind<IStudyPlanRepository>().To<StudyPlanRepository>();
            kernel.Bind<ISubjectRepository>().To<SubjectRepository>();
            kernel.Bind<ISemesterRepository>().To<SemesterRepository>();
            kernel.Bind<IRecordRepository>().To<RecordRepository>();
            kernel.Bind<IProfessorRepository>().To<ProfessorRepository>();


            //Register Services
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<IStudyPlanService>().To<StudyPlanService>();
            kernel.Bind<ISubjectService>().To<SubjectService>();
            kernel.Bind<ISemesterService>().To<SemesterService>();
            kernel.Bind<IRecordService>().To<RecordService>();
            kernel.Bind<IProfessorService>().To<ProfessorService>();


        }        
    }
}
