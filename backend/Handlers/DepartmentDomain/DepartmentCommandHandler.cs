using backend.Models.DepartmentDomain;
using backend.Repositories.DepartmentDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF;
using VDS.RDF.Query;

namespace backend.Handlers.DepartmentDomain
{
    public class DepartmentCommandHandler : IDepartmentCommandHandler
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task CreateDepartment(Department department)
        {
            var id = department.CreateDepartmentId();
            var departmentTriples = new List<Triple>();

            INode subject = _departmentRepository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
            INode predicate = _departmentRepository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode obj = _departmentRepository.GetGraph().GetUriNode(HelperService.CreateUri(DepartmentConstants.CLASS_NAME));
            departmentTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasName = _departmentRepository.GetGraph().GetUriNode(HelperService.CreateUri(DepartmentConstants.NAME_PROP));
            INode objName = _departmentRepository.GetGraph().CreateLiteralNode(department.Name);
            departmentTriples.Add(new Triple(subject, predicateHasName, objName));

            INode predicatehasAbbreviation = _departmentRepository.GetGraph().GetUriNode(HelperService.CreateUri(DepartmentConstants.ABBR_PROP));
            INode objAbbreviation = _departmentRepository.GetGraph().CreateLiteralNode(department.Abbr);
            departmentTriples.Add(new Triple(subject, predicatehasAbbreviation, objAbbreviation));

            await _departmentRepository.CreateAsync(departmentTriples);
        }

        public async Task DeleteDepartment(string departmentId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @id ?p1 ?o. ?s ?p2 @id. } } WHERE { Graph ?g { @id ?p1 ?o. OPTIONAL { ?s ?p2 @id. } } }";
            queryString.SetUri("id", HelperService.CreateUri(departmentId));

            await _departmentRepository.DeleteAsync(queryString.ToString());
        }
    }
}
