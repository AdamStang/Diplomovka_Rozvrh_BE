using backend.Database;
using backend.Handlers.DepartmentDomain;
using backend.Handlers.LessonDomain;
using backend.Handlers.RoomDomain;
using backend.Handlers.SchoolSubjectDomain;
using backend.Handlers.StudyProgrammeDomain;
using backend.Handlers.TeacherDomain;
using backend.Handlers.TimeslotDomain;
using backend.Repositories.DepartmentDomain;
using backend.Repositories.LessonDomain;
using backend.Repositories.RoomDomain;
using backend.Repositories.SchoolSubjectDomain;
using backend.Repositories.StudyProgrammeDomain;
using backend.Repositories.TeacherDomain;
using backend.Repositories.TimeslotDomain;
using backend.Services;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDbContext, DbContext>();
//builder.Services.AddScoped<IHashing, Hashing>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomQueryHandler, RoomQueryHandler>();
builder.Services.AddScoped<IRoomCommandHandler, RoomCommandHandler>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentQueryHandler, DepartmentQueryHandler>();
builder.Services.AddScoped<IDepartmentCommandHandler, DepartmentCommandHandler>();

builder.Services.AddScoped<ISchoolSubjectRepository, SchoolSubjectRepository>();
builder.Services.AddScoped<ISchoolSubjectQueryHandler, SchoolSubjectQueryHandler>();
builder.Services.AddScoped<ISchoolSubjectCommandHandler, SchoolSubjectCommandHandler>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITeacherQueryHandler, TeacherQueryHandler>();
builder.Services.AddScoped<ITeacherCommandHandler, TeacherCommandHandler>();

builder.Services.AddScoped<IStudyProgrammeRepository, StudyProgrammeRepository>();
builder.Services.AddScoped<IStudyProgrammeQueryHandler, StudyProgrammeQueryHandler>();
builder.Services.AddScoped<IStudyProgrammeCommandHandler, StudyProgrammeCommandHandler>();

builder.Services.AddScoped<ITimeslotRepository, TimeslotRepository>();
builder.Services.AddScoped<ITimeslotQueryHandler, TimeslotQueryHandler>();
builder.Services.AddScoped<ITimeslotCommandHandler, TimeslotCommandHandler>();

builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ILessonQueryHandler, LessonQueryHandler>();
builder.Services.AddScoped<ILessonCommandHandler, LessonCommandHandler>();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(option => {
    option.AddDefaultPolicy(builder => {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//builder.Services.AddMvc().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
