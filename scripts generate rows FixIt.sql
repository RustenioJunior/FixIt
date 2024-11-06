-- Generate sample data for Company table
insert into
  company (name, nif, address, email, phone)
select
  'Company ' || i,
  'NIF' || i,
  'Address ' || i,
  'email' || i || '@company.com',
  '123-456-789'
from
  generate_series(1, 100000) as s (i);

-- Generate sample data for Machine_Type table
insert into
  machine_type (description)
select
  'Machine Type ' || i
from
  generate_series(1, 10) as s (i);

-- Generate sample data for Role table
insert into
  role (name, permission, description)
select
  'Role ' || i,
  'Permission ' || i,
  'Description for role ' || i
from
  generate_series(1, 5) as s (i);

-- Generate sample data for Client table
insert into
  client (company_id, name, email, phone)
select
  (i % 1000) + 1,
  'Client ' || i,
  'client' || i || '@example.com',
  '987-654-321'
from
  generate_series(1, 10000) as s (i);

-- Generate sample data for Machine_Model table
insert into
  machine_model (
    machine_type_id,
    model,
    number_hours,
    machine_typology,
    maintenance_date
  )
select
  (i % 10) + 1,
  'Model ' || i,
  (i * 10) % 1000,
  'Typology ' || i,
  current_date - (i % 365)
from
  generate_series(1, 100) as s (i);

-- Generate sample data for Worker table
insert into
  worker (
    role_id,
    name,
    admission_date,
    machine_type,
    verified
  )
select
  (i % 5) + 1,
  'Worker ' || i,
  current_date - (i % 365),
  'Machine Type ' || (i % 10) + 1,
  (i % 2) = 0
from
  generate_series(1, 1000) as s (i);

-- Generate sample data for Machine table
insert into
  machine (
    company_id,
    machine_model_id,
    serial_number,
    number_hours,
    maintenance_user
  )
select
  (i % 1000) + 1,
  (i % 100) + 1,
  'SN' || i,
  (i * 5) % 1000,
  'User ' || (i % 1000) + 1
from
  generate_series(1, 1000) as s (i);

-- Generate sample data for Appointment table
insert into
  appointment (
    date_appointment,
    date_conclusion,
    status,
    client_id,
    machine_id
  )
select
  current_date - (i % 30),
  current_date - (i % 15),
  case
    when i % 3 = 0 then 'Completed'
    else 'Pending'
  end,
  (i % 10000) + 1,
  (i % 1000) + 1
from
  generate_series(1, 1000) as s (i);

-- Generate sample data for Service table
insert into
  service (
    date_start,
    date_conclusion,
    client_signature,
    worker_id
  )
select
  current_date - (i % 30),
  current_date - (i % 15),
  'Signature ' || i,
  (i % 1000) + 1
from
  generate_series(1, 1000) as s (i);

-- Generate sample data for Review table
insert into
  review (service_id, review_star, description)
select
  (i % 1000) + 1,
  (i % 5) + 1,
  'Review description ' || i
from
  generate_series(1, 1000) as s (i);

-- Generate sample data for Login table
insert into
  login (
    email,
    password,
    remember,
    client_id,
    worker_id,
    last_login
  )
select
  'login' || i || '@example.com',
  'password' || i,
  (i % 2) = 0,
  (i % 10000) + 1,
  (i % 1000) + 1,
  current_timestamp - (i % 100) * interval '1 day'
from
  generate_series(1, 1000) as s (i);