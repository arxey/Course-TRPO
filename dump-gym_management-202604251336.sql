--
-- PostgreSQL database cluster dump
--

-- Started on 2026-04-25 13:36:19

\restrict wPfcXo9NotXds7LaXhXQ8VPn1C1BOpUdOPgMcLNcJi5KfmOtAnkeugCOuMdHVPk

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE admin;
ALTER ROLE admin WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:tkLzc7VzKZAiPzCvp+uj8w==$VBvwQZWmylsxZookH9fo6PyXZ7fMhAU5HMK+j2fpRCE=:b74yl/t3z8XLJMj7Mq6qYiBKTo6IqxltQIMW4YJDUTg=';
CREATE ROLE client;
ALTER ROLE client WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:f1gqaaSjG4B7urJt4SMGeA==$6J1rBvwHjnBgNt4XLx/UYAlA8n/ISV4wTel3q4oFPNE=:KBrZ7t0EfQOnxbQCMpnUf7dnq4xwN3/wBDAqoEoxuSU=';
CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:+yOtHdWLs0Eddc1NEKX2xQ==$8bzCYKrn0JWjauJg9sKRivAroEFw8hfhF4kKz6vpmOs=:SXjS1aJ+1/UmkN52d3wCcsZS7xOb2CwkvRVh/WPNFBY=';
CREATE ROLE trainer;
ALTER ROLE trainer WITH NOSUPERUSER INHERIT NOCREATEROLE NOCREATEDB LOGIN NOREPLICATION NOBYPASSRLS PASSWORD 'SCRAM-SHA-256$4096:OGmYuCnlbe95vuU2ZHRacA==$gKQJxw4DD+RBJwHtcfOENN1HvWtLNyfoYPJIi3IFoSk=:jIGluvP+G9asYucLySK5+cHAbrAaYXSkY4gJHoOXuq8=';

--
-- User Configurations
--








\unrestrict wPfcXo9NotXds7LaXhXQ8VPn1C1BOpUdOPgMcLNcJi5KfmOtAnkeugCOuMdHVPk

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict BvAENVOl2Kyo2wyF4FsJmep9ajM55fk7tDlnHtFX01lu4uzDWCy0foEQfm8K6Ua

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

-- Started on 2026-04-25 13:36:19

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-04-25 13:36:19

--
-- PostgreSQL database dump complete
--

\unrestrict BvAENVOl2Kyo2wyF4FsJmep9ajM55fk7tDlnHtFX01lu4uzDWCy0foEQfm8K6Ua

--
-- Database "gym_management" dump
--

--
-- PostgreSQL database dump
--

\restrict XoENBF3IBsjGXw01zXpHCGOZcbbE1wnOLKJLTRO7taShavN0a3JeYemiHDnsaue

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

-- Started on 2026-04-25 13:36:19

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5162 (class 1262 OID 29340)
-- Name: gym_management; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE gym_management WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';


ALTER DATABASE gym_management OWNER TO postgres;

\unrestrict XoENBF3IBsjGXw01zXpHCGOZcbbE1wnOLKJLTRO7taShavN0a3JeYemiHDnsaue
\connect gym_management
\restrict XoENBF3IBsjGXw01zXpHCGOZcbbE1wnOLKJLTRO7taShavN0a3JeYemiHDnsaue

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 246 (class 1255 OID 29521)
-- Name: add_payment(integer, integer, numeric); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.add_payment(IN p_client_id integer, IN p_subscription_id integer, IN p_amount numeric)
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO payments (client_id, subscription_id, amount, payment_date)
    VALUES (p_client_id, p_subscription_id, p_amount, NOW());
END;
$$;


ALTER PROCEDURE public.add_payment(IN p_client_id integer, IN p_subscription_id integer, IN p_amount numeric) OWNER TO postgres;

--
-- TOC entry 243 (class 1255 OID 29517)
-- Name: calculate_subscription_end(date, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.calculate_subscription_end(start_date date, duration_days integer) RETURNS date
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN start_date + duration_days * INTERVAL '1 day';
END;
$$;


ALTER FUNCTION public.calculate_subscription_end(start_date date, duration_days integer) OWNER TO postgres;

--
-- TOC entry 247 (class 1255 OID 29522)
-- Name: check_hall_capacity(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.check_hall_capacity() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
DECLARE
    hall_capacity INT;
    current_count INT;
BEGIN
    SELECT capacity INTO hall_capacity
    FROM halls h
    JOIN sessions s ON h.hall_id = s.hall_id
    WHERE s.session_id = NEW.session_id;

    SELECT COUNT(*) INTO current_count
    FROM session_registrations
    WHERE session_id = NEW.session_id;

    IF current_count >= hall_capacity THEN
        RAISE EXCEPTION 'Зал переполнен! Максимум: % человек.', hall_capacity;
    END IF;

    RETURN NEW;
END;
$$;


ALTER FUNCTION public.check_hall_capacity() OWNER TO postgres;

--
-- TOC entry 245 (class 1255 OID 29520)
-- Name: get_clients_count_for_trainer(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_clients_count_for_trainer(trainer_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    cnt INT;
BEGIN
    SELECT COUNT(DISTINCT r.client_id)
    INTO cnt
    FROM session_registrations r
    JOIN sessions s ON r.session_id = s.session_id
    WHERE s.trainer_id = trainer_id;

    RETURN cnt;
END;
$$;


ALTER FUNCTION public.get_clients_count_for_trainer(trainer_id integer) OWNER TO postgres;

--
-- TOC entry 244 (class 1255 OID 29518)
-- Name: set_subscription_end_date(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_subscription_end_date() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.end_date := calculate_subscription_end(NEW.start_date,
        (SELECT duration_days FROM subscriptions WHERE subscription_id = NEW.subscription_id));
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.set_subscription_end_date() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 232 (class 1259 OID 29440)
-- Name: client_subscriptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.client_subscriptions (
    id integer NOT NULL,
    client_id integer,
    subscription_id integer,
    start_date date DEFAULT CURRENT_DATE,
    end_date date,
    CONSTRAINT client_sub_date_check CHECK (((end_date IS NULL) OR (end_date > start_date)))
);


ALTER TABLE public.client_subscriptions OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 29439)
-- Name: client_subscriptions_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.client_subscriptions_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.client_subscriptions_id_seq OWNER TO postgres;

--
-- TOC entry 5164 (class 0 OID 0)
-- Dependencies: 231
-- Name: client_subscriptions_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.client_subscriptions_id_seq OWNED BY public.client_subscriptions.id;


--
-- TOC entry 220 (class 1259 OID 29356)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    client_id integer NOT NULL,
    full_name character varying(100) NOT NULL,
    phone character varying(20),
    email character varying(100),
    registration_date date DEFAULT CURRENT_DATE
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 29355)
-- Name: clients_client_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.clients_client_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.clients_client_id_seq OWNER TO postgres;

--
-- TOC entry 5167 (class 0 OID 0)
-- Dependencies: 219
-- Name: clients_client_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.clients_client_id_seq OWNED BY public.clients.client_id;


--
-- TOC entry 224 (class 1259 OID 29380)
-- Name: halls; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.halls (
    hall_id integer NOT NULL,
    name character varying(100) NOT NULL,
    capacity integer,
    location character varying(100),
    CONSTRAINT halls_capacity_check CHECK ((capacity > 0))
);


ALTER TABLE public.halls OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 29379)
-- Name: halls_hall_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.halls_hall_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.halls_hall_id_seq OWNER TO postgres;

--
-- TOC entry 5170 (class 0 OID 0)
-- Dependencies: 223
-- Name: halls_hall_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.halls_hall_id_seq OWNED BY public.halls.hall_id;


--
-- TOC entry 237 (class 1259 OID 29498)
-- Name: payments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.payments (
    payment_id integer NOT NULL,
    client_id integer,
    subscription_id integer,
    amount numeric(10,2),
    payment_date timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT payments_amount_check CHECK ((amount > (0)::numeric))
);


ALTER TABLE public.payments OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 29497)
-- Name: payments_payment_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.payments_payment_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.payments_payment_id_seq OWNER TO postgres;

--
-- TOC entry 5173 (class 0 OID 0)
-- Dependencies: 236
-- Name: payments_payment_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.payments_payment_id_seq OWNED BY public.payments.payment_id;


--
-- TOC entry 234 (class 1259 OID 29460)
-- Name: session_registrations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.session_registrations (
    registration_id integer NOT NULL,
    session_id integer,
    client_id integer,
    registration_date date DEFAULT CURRENT_DATE
);


ALTER TABLE public.session_registrations OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 29459)
-- Name: session_registrations_registration_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.session_registrations_registration_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.session_registrations_registration_id_seq OWNER TO postgres;

--
-- TOC entry 5176 (class 0 OID 0)
-- Dependencies: 233
-- Name: session_registrations_registration_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.session_registrations_registration_id_seq OWNED BY public.session_registrations.registration_id;


--
-- TOC entry 228 (class 1259 OID 29401)
-- Name: sessions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sessions (
    session_id integer NOT NULL,
    hall_id integer,
    trainer_id integer,
    type_id integer,
    session_date date NOT NULL,
    day_of_week character varying(15),
    start_time time without time zone NOT NULL,
    end_time time without time zone NOT NULL,
    CONSTRAINT session_time_check CHECK ((end_time > start_time)),
    CONSTRAINT sessions_day_of_week_check CHECK (((day_of_week)::text = ANY ((ARRAY['Понедельник'::character varying, 'Вторник'::character varying, 'Среда'::character varying, 'Четверг'::character varying, 'Пятница'::character varying, 'Суббота'::character varying, 'Воскресенье'::character varying])::text[])))
);


ALTER TABLE public.sessions OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 29400)
-- Name: sessions_session_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sessions_session_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sessions_session_id_seq OWNER TO postgres;

--
-- TOC entry 5179 (class 0 OID 0)
-- Dependencies: 227
-- Name: sessions_session_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sessions_session_id_seq OWNED BY public.sessions.session_id;


--
-- TOC entry 230 (class 1259 OID 29429)
-- Name: subscriptions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subscriptions (
    subscription_id integer NOT NULL,
    name character varying(50) NOT NULL,
    duration_days integer,
    price numeric(10,2),
    CONSTRAINT subscriptions_duration_days_check CHECK ((duration_days > 0)),
    CONSTRAINT subscriptions_price_check CHECK ((price >= (0)::numeric))
);


ALTER TABLE public.subscriptions OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 29428)
-- Name: subscriptions_subscription_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.subscriptions_subscription_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.subscriptions_subscription_id_seq OWNER TO postgres;

--
-- TOC entry 5182 (class 0 OID 0)
-- Dependencies: 229
-- Name: subscriptions_subscription_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.subscriptions_subscription_id_seq OWNED BY public.subscriptions.subscription_id;


--
-- TOC entry 235 (class 1259 OID 29480)
-- Name: trainer_specializations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.trainer_specializations (
    trainer_id integer NOT NULL,
    type_id integer NOT NULL
);


ALTER TABLE public.trainer_specializations OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 29370)
-- Name: trainers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.trainers (
    trainer_id integer NOT NULL,
    full_name character varying(100) NOT NULL,
    phone character varying(20),
    email character varying(100),
    experience_years integer,
    CONSTRAINT trainers_experience_years_check CHECK ((experience_years >= 0))
);


ALTER TABLE public.trainers OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 29369)
-- Name: trainers_trainer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.trainers_trainer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.trainers_trainer_id_seq OWNER TO postgres;

--
-- TOC entry 5186 (class 0 OID 0)
-- Dependencies: 221
-- Name: trainers_trainer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.trainers_trainer_id_seq OWNED BY public.trainers.trainer_id;


--
-- TOC entry 226 (class 1259 OID 29390)
-- Name: training_types; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.training_types (
    type_id integer NOT NULL,
    name character varying(50) NOT NULL,
    description text
);


ALTER TABLE public.training_types OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 29389)
-- Name: training_types_type_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.training_types_type_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.training_types_type_id_seq OWNER TO postgres;

--
-- TOC entry 5189 (class 0 OID 0)
-- Dependencies: 225
-- Name: training_types_type_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.training_types_type_id_seq OWNED BY public.training_types.type_id;


--
-- TOC entry 239 (class 1259 OID 29534)
-- Name: v_active_subscriptions; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.v_active_subscriptions AS
 SELECT c.client_id,
    c.full_name,
    cs.subscription_id,
    s.name AS subscription_name,
    cs.start_date,
    cs.end_date,
        CASE
            WHEN (cs.end_date >= CURRENT_DATE) THEN 'Активна'::text
            ELSE 'Истекла'::text
        END AS status
   FROM ((public.clients c
     JOIN public.client_subscriptions cs ON ((c.client_id = cs.client_id)))
     JOIN public.subscriptions s ON ((cs.subscription_id = s.subscription_id)));


ALTER VIEW public.v_active_subscriptions OWNER TO postgres;

--
-- TOC entry 5191 (class 0 OID 0)
-- Dependencies: 239
-- Name: VIEW v_active_subscriptions; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON VIEW public.v_active_subscriptions IS 'Информация о клиентах и состоянии их подписок.';


--
-- TOC entry 240 (class 1259 OID 29539)
-- Name: v_client_sessions; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.v_client_sessions AS
 SELECT r.registration_id,
    c.full_name AS client,
    tt.name AS training_type,
    t.full_name AS trainer,
    h.name AS hall_name,
    s.day_of_week,
    s.start_time,
    s.end_time,
    r.registration_date
   FROM (((((public.session_registrations r
     JOIN public.clients c ON ((r.client_id = c.client_id)))
     JOIN public.sessions s ON ((r.session_id = s.session_id)))
     JOIN public.halls h ON ((s.hall_id = h.hall_id)))
     JOIN public.trainers t ON ((s.trainer_id = t.trainer_id)))
     JOIN public.training_types tt ON ((s.type_id = tt.type_id)));


ALTER VIEW public.v_client_sessions OWNER TO postgres;

--
-- TOC entry 5193 (class 0 OID 0)
-- Dependencies: 240
-- Name: VIEW v_client_sessions; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON VIEW public.v_client_sessions IS 'Записи клиентов на занятия (расписание с привязкой к клиентам).';


--
-- TOC entry 241 (class 1259 OID 29544)
-- Name: v_payment_history; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.v_payment_history AS
 SELECT p.payment_id,
    c.full_name AS client,
    s.name AS subscription_name,
    p.amount,
    p.payment_date
   FROM ((public.payments p
     JOIN public.clients c ON ((p.client_id = c.client_id)))
     JOIN public.subscriptions s ON ((p.subscription_id = s.subscription_id)))
  ORDER BY p.payment_date DESC;


ALTER VIEW public.v_payment_history OWNER TO postgres;

--
-- TOC entry 5195 (class 0 OID 0)
-- Dependencies: 241
-- Name: VIEW v_payment_history; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON VIEW public.v_payment_history IS 'История всех оплат клиентов с типами подписок.';


--
-- TOC entry 238 (class 1259 OID 29529)
-- Name: v_schedule; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.v_schedule AS
 SELECT s.session_id,
    tt.name AS training_type,
    t.full_name AS trainer,
    h.name AS hall_name,
    h.location,
    s.day_of_week,
    s.start_time,
    s.end_time
   FROM (((public.sessions s
     JOIN public.training_types tt ON ((s.type_id = tt.type_id)))
     JOIN public.trainers t ON ((s.trainer_id = t.trainer_id)))
     JOIN public.halls h ON ((s.hall_id = h.hall_id)))
  ORDER BY s.day_of_week, s.start_time;


ALTER VIEW public.v_schedule OWNER TO postgres;

--
-- TOC entry 5197 (class 0 OID 0)
-- Dependencies: 238
-- Name: VIEW v_schedule; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON VIEW public.v_schedule IS 'Общее расписание занятий по дням недели, тренерам и залам.';


--
-- TOC entry 242 (class 1259 OID 29548)
-- Name: v_trainer_stats; Type: VIEW; Schema: public; Owner: postgres
--

CREATE VIEW public.v_trainer_stats AS
 SELECT t.trainer_id,
    t.full_name AS trainer,
    count(DISTINCT s.session_id) AS sessions_count,
    count(DISTINCT r.client_id) AS total_clients
   FROM ((public.trainers t
     LEFT JOIN public.sessions s ON ((s.trainer_id = t.trainer_id)))
     LEFT JOIN public.session_registrations r ON ((r.session_id = s.session_id)))
  GROUP BY t.trainer_id, t.full_name
  ORDER BY (count(DISTINCT r.client_id)) DESC;


ALTER VIEW public.v_trainer_stats OWNER TO postgres;

--
-- TOC entry 5199 (class 0 OID 0)
-- Dependencies: 242
-- Name: VIEW v_trainer_stats; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON VIEW public.v_trainer_stats IS 'Количество занятий и клиентов по каждому тренеру.';


--
-- TOC entry 4933 (class 2604 OID 29443)
-- Name: client_subscriptions id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_subscriptions ALTER COLUMN id SET DEFAULT nextval('public.client_subscriptions_id_seq'::regclass);


--
-- TOC entry 4926 (class 2604 OID 29359)
-- Name: clients client_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients ALTER COLUMN client_id SET DEFAULT nextval('public.clients_client_id_seq'::regclass);


--
-- TOC entry 4929 (class 2604 OID 29383)
-- Name: halls hall_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.halls ALTER COLUMN hall_id SET DEFAULT nextval('public.halls_hall_id_seq'::regclass);


--
-- TOC entry 4937 (class 2604 OID 29501)
-- Name: payments payment_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments ALTER COLUMN payment_id SET DEFAULT nextval('public.payments_payment_id_seq'::regclass);


--
-- TOC entry 4935 (class 2604 OID 29463)
-- Name: session_registrations registration_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.session_registrations ALTER COLUMN registration_id SET DEFAULT nextval('public.session_registrations_registration_id_seq'::regclass);


--
-- TOC entry 4931 (class 2604 OID 29404)
-- Name: sessions session_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions ALTER COLUMN session_id SET DEFAULT nextval('public.sessions_session_id_seq'::regclass);


--
-- TOC entry 4932 (class 2604 OID 29432)
-- Name: subscriptions subscription_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subscriptions ALTER COLUMN subscription_id SET DEFAULT nextval('public.subscriptions_subscription_id_seq'::regclass);


--
-- TOC entry 4928 (class 2604 OID 29373)
-- Name: trainers trainer_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainers ALTER COLUMN trainer_id SET DEFAULT nextval('public.trainers_trainer_id_seq'::regclass);


--
-- TOC entry 4930 (class 2604 OID 29393)
-- Name: training_types type_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.training_types ALTER COLUMN type_id SET DEFAULT nextval('public.training_types_type_id_seq'::regclass);


--
-- TOC entry 5151 (class 0 OID 29440)
-- Dependencies: 232
-- Data for Name: client_subscriptions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.client_subscriptions (id, client_id, subscription_id, start_date, end_date) FROM stdin;
1	1	2	2025-09-01	2025-11-30
2	2	1	2025-10-01	2025-10-31
3	3	3	2025-07-15	2026-01-15
\.


--
-- TOC entry 5139 (class 0 OID 29356)
-- Dependencies: 220
-- Data for Name: clients; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.clients (client_id, full_name, phone, email, registration_date) FROM stdin;
1	Иван Петров	+7(074)005-59-23	ivan.petrov@gmail.com	2024-01-15
2	Мария Смирнова	+7(739)214-14-94	maria.smirnova@mail.ru	2024-03-10
3	Алексей Кузнецов	+7(550)434-81-77	alex.kuz@yandex.ru	2024-06-20
5	Семён	+7(343)456-81-35	semyon@gmail.com	2025-12-19
\.


--
-- TOC entry 5143 (class 0 OID 29380)
-- Dependencies: 224
-- Data for Name: halls; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.halls (hall_id, name, capacity, location) FROM stdin;
1	Зал №1	25	1 этаж
2	Зал №2	30	1 этаж
3	Зал №3	20	2 этаж
4	Зал №4	15	2 этаж
5	Зал №5	25	3 этаж
6	Зал №6	30	3 этаж
7	Зал №7	40	3 этаж
\.


--
-- TOC entry 5156 (class 0 OID 29498)
-- Dependencies: 237
-- Data for Name: payments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.payments (payment_id, client_id, subscription_id, amount, payment_date) FROM stdin;
1	1	2	6500.00	2025-09-01 10:00:00
2	2	1	2500.00	2025-10-01 09:30:00
3	3	3	12000.00	2025-07-15 12:00:00
\.


--
-- TOC entry 5153 (class 0 OID 29460)
-- Dependencies: 234
-- Data for Name: session_registrations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.session_registrations (registration_id, session_id, client_id, registration_date) FROM stdin;
1	1	1	2025-11-09
2	2	2	2025-11-10
3	3	3	2025-11-11
4	4	1	2025-11-12
5	5	2	2025-11-13
6	2	3	2025-12-19
8	4	3	2026-02-17
9	2	5	2026-02-17
\.


--
-- TOC entry 5147 (class 0 OID 29401)
-- Dependencies: 228
-- Data for Name: sessions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sessions (session_id, hall_id, trainer_id, type_id, session_date, day_of_week, start_time, end_time) FROM stdin;
1	1	1	1	2025-11-10	Понедельник	09:00:00	10:00:00
2	2	2	2	2025-11-11	Вторник	10:00:00	11:00:00
3	3	3	3	2025-11-12	Среда	18:00:00	19:00:00
4	4	1	4	2025-11-13	Четверг	19:00:00	20:00:00
5	5	2	3	2025-11-14	Пятница	17:00:00	18:00:00
\.


--
-- TOC entry 5149 (class 0 OID 29429)
-- Dependencies: 230
-- Data for Name: subscriptions; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.subscriptions (subscription_id, name, duration_days, price) FROM stdin;
1	1 месяц — базовый	30	2500.00
2	3 месяца — стандарт	90	6500.00
3	6 месяцев — премиум	180	12000.00
\.


--
-- TOC entry 5154 (class 0 OID 29480)
-- Dependencies: 235
-- Data for Name: trainer_specializations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.trainer_specializations (trainer_id, type_id) FROM stdin;
1	1
1	4
2	2
2	3
3	3
\.


--
-- TOC entry 5141 (class 0 OID 29370)
-- Dependencies: 222
-- Data for Name: trainers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.trainers (trainer_id, full_name, phone, email, experience_years) FROM stdin;
1	Ольга Иванова	+7(751)662-22-73	olga.ivanova@gmail.com	5
2	Сергей Орлов	+7(123)710-54-59	sergey.orlov@mail.ru	8
3	Анна Соколова	+7(148)170-63-95	anna.sokolova@yandex.ru	3
\.


--
-- TOC entry 5145 (class 0 OID 29390)
-- Dependencies: 226
-- Data for Name: training_types; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.training_types (type_id, name, description) FROM stdin;
1	Йога	Улучшение гибкости и дыхания
2	Кардиотренировка	Интенсивная нагрузка для укрепления сердца
3	Силовая тренировка	Развитие силы и выносливости
4	Пилатес	Укрепление мышц кора и осанки
\.


--
-- TOC entry 5201 (class 0 OID 0)
-- Dependencies: 231
-- Name: client_subscriptions_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.client_subscriptions_id_seq', 3, true);


--
-- TOC entry 5202 (class 0 OID 0)
-- Dependencies: 219
-- Name: clients_client_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.clients_client_id_seq', 7, true);


--
-- TOC entry 5203 (class 0 OID 0)
-- Dependencies: 223
-- Name: halls_hall_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.halls_hall_id_seq', 7, true);


--
-- TOC entry 5204 (class 0 OID 0)
-- Dependencies: 236
-- Name: payments_payment_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.payments_payment_id_seq', 3, true);


--
-- TOC entry 5205 (class 0 OID 0)
-- Dependencies: 233
-- Name: session_registrations_registration_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.session_registrations_registration_id_seq', 9, true);


--
-- TOC entry 5206 (class 0 OID 0)
-- Dependencies: 227
-- Name: sessions_session_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sessions_session_id_seq', 5, true);


--
-- TOC entry 5207 (class 0 OID 0)
-- Dependencies: 229
-- Name: subscriptions_subscription_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.subscriptions_subscription_id_seq', 3, true);


--
-- TOC entry 5208 (class 0 OID 0)
-- Dependencies: 221
-- Name: trainers_trainer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.trainers_trainer_id_seq', 3, true);


--
-- TOC entry 5209 (class 0 OID 0)
-- Dependencies: 225
-- Name: training_types_type_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.training_types_type_id_seq', 4, true);


--
-- TOC entry 4964 (class 2606 OID 29448)
-- Name: client_subscriptions client_subscriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_subscriptions
    ADD CONSTRAINT client_subscriptions_pkey PRIMARY KEY (id);


--
-- TOC entry 4948 (class 2606 OID 29368)
-- Name: clients clients_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_email_key UNIQUE (email);


--
-- TOC entry 4950 (class 2606 OID 29366)
-- Name: clients clients_phone_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_phone_key UNIQUE (phone);


--
-- TOC entry 4952 (class 2606 OID 29364)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (client_id);


--
-- TOC entry 4956 (class 2606 OID 29388)
-- Name: halls halls_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.halls
    ADD CONSTRAINT halls_pkey PRIMARY KEY (hall_id);


--
-- TOC entry 4972 (class 2606 OID 29506)
-- Name: payments payments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_pkey PRIMARY KEY (payment_id);


--
-- TOC entry 4966 (class 2606 OID 29467)
-- Name: session_registrations session_registrations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.session_registrations
    ADD CONSTRAINT session_registrations_pkey PRIMARY KEY (registration_id);


--
-- TOC entry 4968 (class 2606 OID 29469)
-- Name: session_registrations session_registrations_session_id_client_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.session_registrations
    ADD CONSTRAINT session_registrations_session_id_client_id_key UNIQUE (session_id, client_id);


--
-- TOC entry 4960 (class 2606 OID 29412)
-- Name: sessions sessions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT sessions_pkey PRIMARY KEY (session_id);


--
-- TOC entry 4962 (class 2606 OID 29438)
-- Name: subscriptions subscriptions_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subscriptions
    ADD CONSTRAINT subscriptions_pkey PRIMARY KEY (subscription_id);


--
-- TOC entry 4970 (class 2606 OID 29486)
-- Name: trainer_specializations trainer_specializations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainer_specializations
    ADD CONSTRAINT trainer_specializations_pkey PRIMARY KEY (trainer_id, type_id);


--
-- TOC entry 4954 (class 2606 OID 29378)
-- Name: trainers trainers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainers
    ADD CONSTRAINT trainers_pkey PRIMARY KEY (trainer_id);


--
-- TOC entry 4958 (class 2606 OID 29399)
-- Name: training_types training_types_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.training_types
    ADD CONSTRAINT training_types_pkey PRIMARY KEY (type_id);


--
-- TOC entry 4985 (class 2620 OID 29523)
-- Name: session_registrations trg_check_capacity; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trg_check_capacity BEFORE INSERT ON public.session_registrations FOR EACH ROW EXECUTE FUNCTION public.check_hall_capacity();


--
-- TOC entry 4984 (class 2620 OID 29519)
-- Name: client_subscriptions trg_set_end_date; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trg_set_end_date BEFORE INSERT ON public.client_subscriptions FOR EACH ROW EXECUTE FUNCTION public.set_subscription_end_date();


--
-- TOC entry 4976 (class 2606 OID 29449)
-- Name: client_subscriptions client_subscriptions_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_subscriptions
    ADD CONSTRAINT client_subscriptions_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 4977 (class 2606 OID 29454)
-- Name: client_subscriptions client_subscriptions_subscription_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.client_subscriptions
    ADD CONSTRAINT client_subscriptions_subscription_id_fkey FOREIGN KEY (subscription_id) REFERENCES public.subscriptions(subscription_id) ON DELETE CASCADE;


--
-- TOC entry 4982 (class 2606 OID 29507)
-- Name: payments payments_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 4983 (class 2606 OID 29512)
-- Name: payments payments_subscription_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_subscription_id_fkey FOREIGN KEY (subscription_id) REFERENCES public.subscriptions(subscription_id) ON DELETE CASCADE;


--
-- TOC entry 4978 (class 2606 OID 29475)
-- Name: session_registrations session_registrations_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.session_registrations
    ADD CONSTRAINT session_registrations_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 4979 (class 2606 OID 29470)
-- Name: session_registrations session_registrations_session_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.session_registrations
    ADD CONSTRAINT session_registrations_session_id_fkey FOREIGN KEY (session_id) REFERENCES public.sessions(session_id) ON DELETE CASCADE;


--
-- TOC entry 4973 (class 2606 OID 29413)
-- Name: sessions sessions_hall_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT sessions_hall_id_fkey FOREIGN KEY (hall_id) REFERENCES public.halls(hall_id) ON DELETE CASCADE;


--
-- TOC entry 4974 (class 2606 OID 29418)
-- Name: sessions sessions_trainer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT sessions_trainer_id_fkey FOREIGN KEY (trainer_id) REFERENCES public.trainers(trainer_id) ON DELETE SET NULL;


--
-- TOC entry 4975 (class 2606 OID 29423)
-- Name: sessions sessions_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sessions
    ADD CONSTRAINT sessions_type_id_fkey FOREIGN KEY (type_id) REFERENCES public.training_types(type_id) ON DELETE SET NULL;


--
-- TOC entry 4980 (class 2606 OID 29487)
-- Name: trainer_specializations trainer_specializations_trainer_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainer_specializations
    ADD CONSTRAINT trainer_specializations_trainer_id_fkey FOREIGN KEY (trainer_id) REFERENCES public.trainers(trainer_id) ON DELETE CASCADE;


--
-- TOC entry 4981 (class 2606 OID 29492)
-- Name: trainer_specializations trainer_specializations_type_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainer_specializations
    ADD CONSTRAINT trainer_specializations_type_id_fkey FOREIGN KEY (type_id) REFERENCES public.training_types(type_id) ON DELETE CASCADE;


--
-- TOC entry 5163 (class 0 OID 0)
-- Dependencies: 232
-- Name: TABLE client_subscriptions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.client_subscriptions TO admin;


--
-- TOC entry 5165 (class 0 OID 0)
-- Dependencies: 231
-- Name: SEQUENCE client_subscriptions_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.client_subscriptions_id_seq TO admin;


--
-- TOC entry 5166 (class 0 OID 0)
-- Dependencies: 220
-- Name: TABLE clients; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.clients TO admin;
GRANT SELECT ON TABLE public.clients TO trainer;


--
-- TOC entry 5168 (class 0 OID 0)
-- Dependencies: 219
-- Name: SEQUENCE clients_client_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.clients_client_id_seq TO admin;


--
-- TOC entry 5169 (class 0 OID 0)
-- Dependencies: 224
-- Name: TABLE halls; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.halls TO admin;
GRANT SELECT ON TABLE public.halls TO trainer;
GRANT SELECT ON TABLE public.halls TO client;


--
-- TOC entry 5171 (class 0 OID 0)
-- Dependencies: 223
-- Name: SEQUENCE halls_hall_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.halls_hall_id_seq TO admin;


--
-- TOC entry 5172 (class 0 OID 0)
-- Dependencies: 237
-- Name: TABLE payments; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.payments TO admin;


--
-- TOC entry 5174 (class 0 OID 0)
-- Dependencies: 236
-- Name: SEQUENCE payments_payment_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.payments_payment_id_seq TO admin;


--
-- TOC entry 5175 (class 0 OID 0)
-- Dependencies: 234
-- Name: TABLE session_registrations; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.session_registrations TO admin;
GRANT SELECT,UPDATE ON TABLE public.session_registrations TO trainer;
GRANT SELECT,INSERT,DELETE ON TABLE public.session_registrations TO client;


--
-- TOC entry 5177 (class 0 OID 0)
-- Dependencies: 233
-- Name: SEQUENCE session_registrations_registration_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.session_registrations_registration_id_seq TO admin;


--
-- TOC entry 5178 (class 0 OID 0)
-- Dependencies: 228
-- Name: TABLE sessions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sessions TO admin;
GRANT SELECT ON TABLE public.sessions TO trainer;
GRANT SELECT ON TABLE public.sessions TO client;


--
-- TOC entry 5180 (class 0 OID 0)
-- Dependencies: 227
-- Name: SEQUENCE sessions_session_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.sessions_session_id_seq TO admin;


--
-- TOC entry 5181 (class 0 OID 0)
-- Dependencies: 230
-- Name: TABLE subscriptions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.subscriptions TO admin;


--
-- TOC entry 5183 (class 0 OID 0)
-- Dependencies: 229
-- Name: SEQUENCE subscriptions_subscription_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.subscriptions_subscription_id_seq TO admin;


--
-- TOC entry 5184 (class 0 OID 0)
-- Dependencies: 235
-- Name: TABLE trainer_specializations; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.trainer_specializations TO admin;


--
-- TOC entry 5185 (class 0 OID 0)
-- Dependencies: 222
-- Name: TABLE trainers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.trainers TO admin;


--
-- TOC entry 5187 (class 0 OID 0)
-- Dependencies: 221
-- Name: SEQUENCE trainers_trainer_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.trainers_trainer_id_seq TO admin;


--
-- TOC entry 5188 (class 0 OID 0)
-- Dependencies: 226
-- Name: TABLE training_types; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.training_types TO admin;
GRANT SELECT ON TABLE public.training_types TO trainer;
GRANT SELECT ON TABLE public.training_types TO client;


--
-- TOC entry 5190 (class 0 OID 0)
-- Dependencies: 225
-- Name: SEQUENCE training_types_type_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.training_types_type_id_seq TO admin;


--
-- TOC entry 5192 (class 0 OID 0)
-- Dependencies: 239
-- Name: TABLE v_active_subscriptions; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT ON TABLE public.v_active_subscriptions TO trainer;
GRANT SELECT ON TABLE public.v_active_subscriptions TO client;
GRANT ALL ON TABLE public.v_active_subscriptions TO admin;


--
-- TOC entry 5194 (class 0 OID 0)
-- Dependencies: 240
-- Name: TABLE v_client_sessions; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT ON TABLE public.v_client_sessions TO trainer;
GRANT SELECT ON TABLE public.v_client_sessions TO client;
GRANT ALL ON TABLE public.v_client_sessions TO admin;


--
-- TOC entry 5196 (class 0 OID 0)
-- Dependencies: 241
-- Name: TABLE v_payment_history; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT ON TABLE public.v_payment_history TO trainer;
GRANT SELECT ON TABLE public.v_payment_history TO client;
GRANT ALL ON TABLE public.v_payment_history TO admin;


--
-- TOC entry 5198 (class 0 OID 0)
-- Dependencies: 238
-- Name: TABLE v_schedule; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT ON TABLE public.v_schedule TO trainer;
GRANT SELECT ON TABLE public.v_schedule TO client;
GRANT ALL ON TABLE public.v_schedule TO admin;


--
-- TOC entry 5200 (class 0 OID 0)
-- Dependencies: 242
-- Name: TABLE v_trainer_stats; Type: ACL; Schema: public; Owner: postgres
--

GRANT SELECT ON TABLE public.v_trainer_stats TO trainer;
GRANT SELECT ON TABLE public.v_trainer_stats TO client;
GRANT ALL ON TABLE public.v_trainer_stats TO admin;


--
-- TOC entry 2121 (class 826 OID 29528)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT SELECT ON TABLES TO trainer;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT SELECT ON TABLES TO client;


-- Completed on 2026-04-25 13:36:19

--
-- PostgreSQL database dump complete
--

\unrestrict XoENBF3IBsjGXw01zXpHCGOZcbbE1wnOLKJLTRO7taShavN0a3JeYemiHDnsaue

-- Completed on 2026-04-25 13:36:19

--
-- PostgreSQL database cluster dump complete
--

