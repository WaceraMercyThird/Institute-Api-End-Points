using AutoMapper;
using Institute.Datas.Models;
using Institute.Dtos;

namespace Institute.Helper
{



    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<StudentDto, Student>()
                .ForMember(x => x.Novel, opt => opt.MapFrom(source => source.Textbook))
                .ReverseMap(); //Map from Student Object to StudentDisplayInfo Object
        }


        internal object Map<T>(StudentDto student)
        {
            throw new NotImplementedException();
        }
    }
}
