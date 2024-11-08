create table company (
  id bigint primary key generated always as identity,
  name varchar(150) not null,
  nif bigint not null,
  address text,
  email varchar(150),
  phone varchar(25),
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table client (
  id bigint primary key generated always as identity,
  company_id bigint references company (id),
  name varchar(150) not null,
  email varchar(150),
  phone varchar(25),
  active boolean default true,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table machine_type (
  id bigint primary key generated always as identity,
  description text not null,
  active boolean default true,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table machine_model (
  id bigint primary key generated always as identity,
  machine_type_id bigint references machine_type (id),
  model text not null,
  machine_typology varchar(150),
  maintenance_date date,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table machine (
  id bigint primary key generated always as identity,
  company_id bigint references company (id),
  machine_model_id bigint references machine_model (id),
  serial_number varchar(150) not null,
  number_hours int,
  active boolean default true,
  maintenance_user varchar(150),
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table appointment (
  id bigint primary key generated always as identity,
  client_id bigint references client (id),
  machine_id bigint references machine (id),
  date_appointment date not null,
  date_conclusion date,
  status text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table role (
  id bigint primary key generated always as identity,
  name varchar(150) not null,
  permission varchar(150),
  description text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table worker (
  id bigint primary key generated always as identity,
  role_id bigint references role (id),
  name varchar(150) not null,
  admission_date date,
  machine_type varchar(150),
  verified boolean default false,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table service (
  id bigint primary key generated always as identity,
  worker_id bigint references worker (id),
  date_start date not null,
  date_conclusion date,
  client_signature varchar(150),
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table review (
  id bigint primary key generated always as identity,
  service_id bigint references service (id),
  review_star int,
  description text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table login (
  id bigint primary key generated always as identity,
  client_id bigint references client (id),
  worker_id bigint references worker (id),
  email varchar(150) not null,
  password text not null,
  remember boolean default false,
  last_login timestamp with time zone default now(),
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);