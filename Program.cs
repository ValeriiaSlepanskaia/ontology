using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;
using MoreLinq;

namespace OntologyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int countClass = 0;
            int countInstance = 0;
            
            OntologyGraph g = new OntologyGraph();
            FileLoader.Load(g, "games.owl");

            OntologyClass someClass = g.CreateOntologyClass(new Uri("http://www.w3.org/2002/07/owl#Thing"));
            OntologyProperty property = g.CreateOntologyProperty(new Uri("http://www.semanticweb.org/user/ontologies/2018/10/games#hasDifficulty"));
            //  OntologyClass indirectClass = g.CreateOntologyClass(new Uri("http://www.semanticweb.org/user/ontologies/2018/10/games#Solitairer"));
            //  OntologyClass superClass=g.CreateOntologyClass(new Uri("http://www.semanticweb.org/user/ontologies/2018/10/games#Game"));
            int count = 0;

            foreach (OntologyClass c in someClass.SubClasses)
            {
                count++;
                countClass++;
       Console.WriteLine(count+")"+
                "Класс онтологии: " + c.Resource.ToString());

                if (c.Instances.Count() != 0)
               {
                    foreach (var instance in c.Instances)
                    {
                        countInstance++;

                        Console.WriteLine("Экземпляр онтологии: " + instance.Resource.ToString());
                    }
                }
           
                OntologyProperty[] properties = c.IsDomainOf.SelectMany(x => EnumerateProperties(x)).DistinctBy(x => x.ToString()).ToArray();
                properties.ForEach(p => Console.WriteLine("Cвойства данного класса: "+p));
            }
            Console.WriteLine("");
            Console.WriteLine("Всего классов в онтологии: " + countClass);
            Console.WriteLine("");
            Console.WriteLine("Всего примеров в онтологии: " + countInstance);
            Console.WriteLine("");
            Console.WriteLine("Класс онтологии, содержащий hasDifficulty: ");
            OntologyClass[] classes = property.Domains.SelectMany(x => EnumerateClasses(x)).DistinctBy(x => x.ToString()).ToArray();
            classes.ForEach(c => Console.WriteLine(c));

            Console.ReadKey();

        }

        private static IEnumerable<OntologyClass> EnumerateClasses(OntologyClass ontologyClass)
        {
            return ontologyClass.SubClasses.SelectMany(x => EnumerateClasses(x)).Concat(new[] { ontologyClass });
        }

        private static IEnumerable<OntologyProperty> EnumerateProperties(OntologyProperty ontologyProperty)
        {
            return ontologyProperty.SubProperties.SelectMany(x => EnumerateProperties(x)).Concat(new[] { ontologyProperty });
        }
        
    }
}

