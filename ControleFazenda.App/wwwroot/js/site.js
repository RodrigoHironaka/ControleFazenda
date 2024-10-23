
//Abrir Modal---------------------------------------------------------------------------
function abrirModal(selector, titulo, rota) {
    $(document).on(TipoClick().toString(), selector, function () {
        var id = $(this).attr("data-id");
        $('table').DataTable().destroy();
        $("#modal").load(rota + id, function () {
            $(".modal-title").html(titulo);
            $("#modal").modal("show");
        });
    });
}
function TipoClick() {
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        return 'touchstart';
    } else {
        return 'click';
    }
}
//--------------------------------------------------------------------------------------

//Salvar--------------------------------------------------------------------------------
function SalvarQuandoModal(formId, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: $("#" + formId).serialize(),
        success: function (response) {
            if (response.success) {
                // Sucesso: fechar a modal e atualizar a página
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso!',
                    text: 'Operação realizada com sucesso.',
                    timer: 5000
                }).then(function () {
                    $('#modal').modal('hide');
                    // Após o popup ser fechado, recarrega a página
                    window.location.reload();
                });
            }
            else if (response.isModelState) {
                // Erros de validação: atualizar os spans de validação na modal
                $.each(response.errors, function (key, value) {
                    var errorMessage = $("<span class='text-danger'></span>").text(value);
                    $("#" + key).siblings(".text-danger").remove(); // Remove erros antigos, se houver
                    $("#" + key).parent().append(errorMessage); // Adiciona o novo erro
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.errors,
                });
            }
        }
    });
}

function SalvarQuandoNaoModal(formId, url) {
    $.ajax({
        type: "POST",
        url: url,
        data: $("#" + formId).serialize(),
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso!',
                    text: 'Operação realizada com sucesso.',
                    timer: 5000
                }).then(function () {
                    window.history.back();
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.errors,
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Ocorreu um erro ao enviar o formulário.',
            });
        }
    });
}

function SalvarQuandoModalComArquivo(formId, url) {
    // Cria um objeto FormData para enviar os dados do formulário
    var formData = new FormData($("#" + formId)[0]);

    $.ajax({
        type: "POST",
        url: url,
        data: formData,
        contentType: false, // Necessário para evitar que o jQuery defina o contentType automaticamente
        processData: false, // Impede que o jQuery processe os dados
        success: function (response) {
            if (response.success) {
                // Sucesso: fechar a modal e atualizar a página
                Swal.fire({
                    icon: 'success',
                    title: 'Sucesso!',
                    text: 'Operação realizada com sucesso.',
                    timer: 5000
                }).then(function () {
                    $('#modal').modal('hide');
                    // Após o popup ser fechado, recarrega a página
                    window.location.reload();
                });
            }
            else if (response.isModelState) {
                // Erros de validação: atualizar os spans de validação na modal
                $.each(response.errors, function (key, value) {
                    var errorMessage = $("<span class='text-danger'></span>").text(value);
                    $("#" + key).siblings(".text-danger").remove(); // Remove erros antigos, se houver
                    $("#" + key).parent().append(errorMessage); // Adiciona o novo erro
                });
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.errors,
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: 'Ocorreu um erro ao enviar o arquivo. Por favor, tente novamente.',
            });
        }
    });
}

//-------------------------------------------------------------------------------------

//Excluir------------------------------------------------------------------------------
function confirmarExclusao(id, url, sucessoCallback) {
    Swal.fire({
        title: 'Deseja realmente excluir?',
        text: "Este processo não pode ser revertido!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, pode excluir!',
        cancelButtonText: 'Fechar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                method: "POST",
                url: url,
                data: { id: id },
                success: sucessoCallback,
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Não é possível excluir, por favor inative o registro!',
                    })
                }
            });
        }
    })
}
function Excluir(id, url) {
    confirmarExclusao(id, url, function () {
        Swal.fire({
            icon: 'success',
            title: 'Registro excluído!',
            text: 'O registro foi excluído com sucesso.',
            timer: 5000
        }).then(function () {
            location.reload();
        });
    });
}
//-------------------------------------------------------------------------------------

function InitializeDataTable(selector, options = {}) {
    const defaultOptions = {
        language: {
            url: '/json/pt-BR_Datatables.json',
        },
        layout: {
            topStart: {
                buttons: [
                    {
                        extend: 'print',
                        exportOptions: {
                            columns: ':visible' // Somente colunas visíveis
                        }
                    },
                    {
                        extend: 'copy',
                        exportOptions: {
                            columns: ':visible' // Somente colunas visíveis
                        }
                    },
                    {
                        extend: 'excel',
                        exportOptions: {
                            columns: ':visible' // Somente colunas visíveis
                        }
                    },
                    {
                        extend: 'pdf',
                        exportOptions: {
                            columns: ':visible' // Somente colunas visíveis
                        }
                    },
                    'colvis' // Botão para alternar a visibilidade das colunas
                ]
            }
        },
        responsive: true,
        rowReorder: {
            selector: 'td:nth-child(2)'
        },
        //select: {
        //    style: 'multi'
        //}
        //layout: {
        //SIMPLIFICADO
        //    topStart: {
        //        buttons: [ 'print', 'copy', 'excel', 'pdf', 'colvis']
        //    }
        //}
    };

    // Mescla as opções padrão com as opções fornecidas
    const finalOptions = { ...defaultOptions, ...options };

    // Inicializa a tabela
    return new DataTable(selector, finalOptions);
}

function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#Endereco_Logradouro").val("");
            $("#Endereco_Bairro").val("");
            $("#Endereco_Cidade").val("");
            $("#Endereco_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#Endereco_Cep").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#Endereco_Logradouro").val("...");
                    $("#Endereco_Bairro").val("...");
                    $("#Endereco_Cidade").val("...");
                    $("#Endereco_Estado").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#Endereco_Logradouro").val(dados.logradouro);
                                $("#Endereco_Bairro").val(dados.bairro);
                                $("#Endereco_Cidade").val(dados.localidade);
                                $("#Endereco_Estado").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}

$(document).ready(function () {
    $("#msg_box").fadeOut(2500);
});

function abrirArquivoPorId(id) {
    const url = `/abrir-arquivo/${id}`; // URL com rota

    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {

        },
        error: function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: xhr.responseText,
            });
        }
    });
}