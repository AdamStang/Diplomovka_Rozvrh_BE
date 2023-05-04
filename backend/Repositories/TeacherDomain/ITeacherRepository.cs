﻿using VDS.RDF;

namespace backend.Repositories.TeacherDomain
{
    public interface ITeacherRepository
    {
        Task CreateAsync(List<Triple> triples);
        Task DeleteAsync(string query);
        Task<object> GetAsync(string query);
        Graph GetGraph();
        Task UpdateAsync(List<Triple> newTriples, List<Triple> previousTriples);
    }
}