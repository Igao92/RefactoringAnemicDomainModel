﻿using FluentNHibernate;
using FluentNHibernate.Mapping;
using Logic.Customers;

namespace Logic.Movies
{
    public class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);

            DiscriminateSubClassesOnColumn("LicensingModel");

            Map(x => x.Name);
            Map(Reveal.Member<Movie>("LicensingModel")).CustomType<int>();
        }

        public class TwoDaysMovieMap : SubclassMap<TwoDaysMovie>
        {
            public TwoDaysMovieMap()
            {
                DiscriminatorValue((int)LicensingModel.TwoDays);
                //DiscriminatorValue(1);
            }
        }

        public class LifeLongMovieMap : SubclassMap<LifeLongMovie>
        {
            public LifeLongMovieMap()
            {
                DiscriminatorValue((int)LicensingModel.LifeLong);
                //DiscriminatorValue(2);
            }
        }
    }
}
