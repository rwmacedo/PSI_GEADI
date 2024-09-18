USE ADIMPLENCIA;

CREATE TABLE BaseMensalFinal (
    ctr                VARCHAR(255) NULL,
    co_ope             CHAR(4) NULL,
    cpf_cnpj           VARCHAR(255) NULL,
    ic_caixa           INT NULL,
    tp_pessoa          CHAR(4) NULL,
    co_sis             CHAR(4) NULL,
    unidade            CHAR(4) NULL,
    co_ag_relac        CHAR(4) NULL,
    dt_conce           DATE NULL,
    DD_VENCIMENTO_CONTRATO INT NULL,
    vlr_conce          DECIMAL(18, 2) NULL,
    posicao            INT NULL,
    da_ini             INT NULL,
    dt_mov             DATE NULL,
    da_atual           INT NULL,
    nu_tabela_atual    INT NULL,
    base_calculo       DECIMAL(18, 2) NULL,
    rat_prov           VARCHAR(255) NULL,
    rat_hh             BIT NULL,
    co_mod             VARCHAR(255) NULL,
    cart               VARCHAR(255) NULL,
    co_cart            CHAR(4) NULL,
    co_seg             INT NULL,
    no_seg             VARCHAR(6) NULL,
    co_segger          INT NULL,
    co_segger_gp       INT NULL,
    co_segad           VARCHAR(3) NULL,
    rat_h5             BIT NULL,
    rat_h6             BIT NULL,
    ic_atacado         BIT NULL,
    ic_reg             BIT NULL,
    ic_rj              BIT NULL,
    ic_honrado         BIT NULL,
    am_honrado         BIT NULL
);


INSERT INTO BaseMensalFinal
SELECT 
    ctr,
    co_ope,
    cpf_cnpj,
    ic_caixa,
    tp_pessoa,
    co_sis,
    unidade,
    co_ag_relac,
    dt_conce,
    DD_VENCIMENTO_CONTRATO, 
    ISNULL(CAST(vlr_conce AS DECIMAL(18,2)), 0) AS vlr_conce,  
    posicao,
    da_ini,
    dt_mov,
    da_atual,
    nu_tabela_atual,
    ISNULL(CAST(base_calculo AS DECIMAL(18,2)), 0) AS base_calculo,  
    rat_prov,
    rat_hh,
    co_mod,
    cart,
    co_cart,
    co_seg,
    no_seg,
    co_segger,
    co_segger_gp,
    co_segad,
    rat_h5,
    rat_h6,
    ic_atacado,
    ic_reg,
    ic_rj,
    ic_honrado,
    am_honrado
FROM BaseMensalTemp
WHERE vlr_conce IS NOT NULL 
AND base_calculo IS NOT NULL;

--- Criar índices

CREATE INDEX idx_unidade ON BaseMensalFinal (unidade);
CREATE INDEX idx_dt_conce ON BaseMensalFinal (dt_conce);


--- Exemplos de consultas
--- Sumarização por Unidade
SELECT unidade, SUM(vlr_conce) AS total_conce, COUNT(*) AS total_registros
FROM BaseMensalFinal
GROUP BY unidade;



