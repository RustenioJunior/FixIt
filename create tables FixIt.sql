create table company (
  id bigint primary key generated always as identity,
  name text not null,
  nif text not null,
  address text,
  email text,
  phone text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table client (
  id bigint primary key generated always as identity,
  company_id bigint references company (id),
  name text not null,
  email text,
  phone text,
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
  number_hours int,
  machine_typology text,
  maintenance_date date,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table machine (
  id bigint primary key generated always as identity,
  company_id bigint references company (id),
  machine_model_id bigint references machine_model (id),
  serial_number text not null,
  number_hours int,
  active boolean default true,
  maintenance_user text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table appointment (
  id bigint primary key generated always as identity,
  date_appointment date not null,
  date_conclusion date,
  status text,
  client_id bigint references client (id),
  machine_id bigint references machine (id),
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table role (
  id bigint primary key generated always as identity,
  name text not null,
  permission text,
  description text,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table worker (
  id bigint primary key generated always as identity,
  role_id bigint references role (id),
  name text not null,
  admission_date date,
  machine_type text,
  verified boolean default false,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);

create table service (
  id bigint primary key generated always as identity,
  date_start date not null,
  date_conclusion date,
  client_signature text,
  worker_id bigint references worker (id),
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
  email text not null,
  password text not null,
  remember boolean default false,
  client_id bigint references client (id),
  worker_id bigint references worker (id),
  last_login timestamp with time zone,
  create_date timestamp with time zone default now(),
  modify_date timestamp with time zone default now()
);