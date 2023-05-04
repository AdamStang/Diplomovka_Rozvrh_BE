using backend.Handlers.SchoolSubjectDomain;
using backend.Models.SchoolSubjectDomain;
using backend.Repositories.StudyProgrammeDomain;
using backend.Services;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF;
using backend.Models.StudyProgrammeDomain;

namespace backend.Handlers.StudyProgrammeDomain
{
    public class StudyProgrammeCommandHandler : IStudyProgrammeCommandHandler
    {
        private readonly IStudyProgrammeRepository _repository;

        public StudyProgrammeCommandHandler(IStudyProgrammeRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateStudyProgramme(StudyProgramme studyProgramme)
        {
            var id = studyProgramme.CreateStudyProgrammeId();
            var studyProgrammeTriples = new List<Triple>();

            INode subject = _repository.GetGraph().CreateUriNode(HelperService.CreateUri(id));
            INode predicate = _repository.GetGraph().CreateUriNode(new Uri(RdfSpecsHelper.RdfType));
            INode obj = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.CLASS_NAME));
            studyProgrammeTriples.Add(new Triple(subject, predicate, obj));

            INode predicateHasName = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.NAME_PROP));
            INode objName = _repository.GetGraph().CreateLiteralNode(studyProgramme.Name);
            studyProgrammeTriples.Add(new Triple(subject, predicateHasName, objName));

            INode predicatehasAbbreviation = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.ABBR_PROP));
            INode objAbbreviation = _repository.GetGraph().CreateLiteralNode(studyProgramme.Abbr);
            studyProgrammeTriples.Add(new Triple(subject, predicatehasAbbreviation, objAbbreviation));

            INode predicatehasGrade = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.GRADE_PROP));
            INode objGrade = _repository.GetGraph().CreateLiteralNode(studyProgramme.Grade.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            studyProgrammeTriples.Add(new Triple(subject, predicatehasGrade, objGrade));

            INode predicatehasNumberOfStudents = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.NUM_OF_STUDS_PROP));
            INode objNumberOfStudents = _repository.GetGraph().CreateLiteralNode(studyProgramme.NumberOfStudents.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            studyProgrammeTriples.Add(new Triple(subject, predicatehasNumberOfStudents, objNumberOfStudents));

            INode predicatehasNumberOfGroups = _repository.GetGraph().GetUriNode(HelperService.CreateUri(StudyProgrammeConstants.NUM_OF_GROUPS_PROP));
            INode objNumberOfGroups = _repository.GetGraph().CreateLiteralNode(studyProgramme.NumberOfGroups.ToString(), new Uri(XmlSpecsHelper.XmlSchemaDataTypeDecimal));
            studyProgrammeTriples.Add(new Triple(subject, predicatehasNumberOfGroups, objNumberOfGroups));

            await _repository.CreateAsync(studyProgrammeTriples);
        }

        public async Task DeleteStudyProgramme(string studyProgrammeId)
        {
            var queryString = new SparqlParameterizedString();

            queryString.CommandText = "DELETE { GRAPH ?g { @id ?p ?o } } WHERE { Graph ?g { @id ?p ?o } }";
            queryString.SetUri("id", HelperService.CreateUri(studyProgrammeId));

            await _repository.DeleteAsync(queryString.ToString());
        }
    }
}
