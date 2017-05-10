from pyspark.sql import DataFrame
trials = reduce(DataFrame.drop, ['Description','Summary'], spark.read.json("wasbs:///clinical-trials-json"))
trials.printSchema()
trials.stat.crosstab("Gender","Phase").show()