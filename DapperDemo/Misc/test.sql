-- nth element first and then remaining table
SELECT
    [employee_id],
    [first_name],
    [last_name],
    [salary]
FROM hcm.employees
WHERE employee_id = 103
UNION ALL
SELECT TOP 9
    [employee_id],
    [first_name],
    [last_name],
    [salary]
FROM hcm.employees
WHERE employee_id <> 103;

-- nth highest salary 3rd in this case
SELECT TOP 1
    salary
FROM (
         SELECT DISTINCT TOP 3
             salary
         FROM hcm.employees
         ORDER BY salary DESC) AS RESULT
ORDER BY salary;
