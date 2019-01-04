<Query Kind="Program" />

void Main()
{
	var list = new List< Foo > {
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 1, Section = 1, Category = 1 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 2, Section = 1, Category = 1 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 3, Section = 2, Category = 1 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 4, Section = 2, Category = 1 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 5, Section = 3, Category = 2 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 6, Section = 3, Category = 2 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 7, Section = 4, Category = 2 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 8, Section = 4, Category = 2 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 9, Section = 5, Category = 2 },
		new Foo { FirmId = 1, Num = 1, Answer = 2, Question = 10, Section = 5, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 1, Section = 1, Category = 1 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 2, Section = 1, Category = 1 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 3, Section = 2, Category = 1 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 4, Section = 2, Category = 1 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 5, Section = 3, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 6, Section = 3, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 7, Section = 4, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 8, Section = 4, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 9, Section = 5, Category = 2 },
		new Foo { FirmId = 1, Num = 2, Answer = 3, Question = 10, Section = 5, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 1, Section = 1, Category = 1 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 2, Section = 1, Category = 1 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 3, Section = 2, Category = 1 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 4, Section = 2, Category = 1 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 5, Section = 3, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 6, Section = 3, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 7, Section = 4, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 8, Section = 4, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 9, Section = 5, Category = 2 },
		new Foo { FirmId = 2, Num = 1, Answer = 5, Question = 10, Section = 5, Category = 2 },
	};
	
	var res = from a in list
			  group a by new { FirmId = a.FirmId, EmployeeId = a.Num } into firms
			  select new {
			      FirmId = firms.Key.FirmId,
				  EmployeeId = firms.Key.EmployeeId,
				  
				  Categories = from c in firms
				  group c by c.Category into cat
				  select new {
				    CategoryId = cat.Key,
					Sections = from s in cat
					group s by s.Section into sec
					select new {
					    SectionId = sec.Key,
						Answer = sec.Sum( s => s.Answer )
					}
				  }
				  
			  }
			  ;
			  
	foreach ( var firm in res ) {
	
		"=============".Dump();
		$"Фирма: {firm.FirmId}".Dump();
		$"Сотрудник: {firm.EmployeeId}".Dump();
		"=============".Dump();
		

		foreach ( var category in firm.Categories ) {
			
			"".Dump();
			$"Категория: {category.CategoryId}".Dump();
			"..........".Dump();
			
			foreach ( var section in category.Sections ) {
				
				$"Секция: {section.SectionId}".Dump();
				$"Сумма: {section.Answer}".Dump();
				"".Dump();
			}
		}
		
	}
}

// Define other methods and classes here
class Foo
{
	public int FirmId { get; set; }
	public int Num { get; set; }
	public int Answer { get; set; }
	
	public int Category { get; set; }
	public int Section { get; set; }
	public int Question { get; set; }
	
}