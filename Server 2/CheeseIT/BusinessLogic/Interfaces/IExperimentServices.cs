using System.Threading.Tasks;
using CheeseIT.Models;

namespace CheeseIT.BusinessLogic.Interfaces
{
    public interface IExperimentServices
    {
        Task<Experiment> GetCurrentExperiment();
        void ValidateMeasure(Measurement measure);
    }
}