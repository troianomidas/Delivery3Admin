var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};

window.Delivery3Theme = {
    Init:  function(){
        KTApp.init();
        KTMenu.init();
    }
}

window.Modal = {
    Close: function (modal) {
        $("#" + modal).modal("hide");
    },
    Show: function (modal) {
        $("#" + modal).modal("show");
    },
}

window.InputMask = {
    Money: function () {
        $('.money').mask('#.##0,00', {reverse: true});
    },
    Number: function () {
        $(".number").mask("999999999");
    },
}

window.Catalog = {
    GetActiveAvailability: function () {
        return $(".nav-availability .nav-item a.active")[0].text;
    }
}

window.DataPicker = {
    Order: function (field, callback) {
        flatpickr($(field), {
            enableTime: false,
            noCalendar: false,           
            dateFormat: "d/m/Y",
            locale: "pt",
            onChange: function (date, dateStr, e) {
                GLOBAL.DotNetReference.invokeMethodAsync(callback, {
                    Input: e.element.name,
                    DateStr: dateStr
                });
            }
        });
    },
    OrderCreate: function (field, callback) {
        flatpickr($(field), {
            enableTime: false,
            noCalendar: false,
            inline: true,
            position: "auto center",
            defaultDate: "today",
            dateFormat: "d/m/Y",
            locale: "pt",
            onChange: function (date, dateStr, e) {
                GLOBAL.DotNetReference.invokeMethodAsync(callback, {
                    Input: e.element.name,
                    DateStr: dateStr
                });
            }
        });
    },
    Financial: function (field) {
        flatpickr($(field), {
            plugins: [
                monthSelectPlugin({
                    shorthand: false,
                    longhand: true,
                    altInput: true,
                    altFormat: "F, \\de Y",
                    dateFormat: "F, \\de Y",
                    theme: "light",
                })
            ],
            enableTime: false,
            noCalendar: false,
            altInput: false,
            altFormat: "F, \\de Y",
            dateFormat: "Y-m-d",
            locale: "pt",
            maxDate: "today",
            defaultDate: "today",
            position: "auto center",
            onChange: function (date, dateStr, e) {
                GLOBAL.DotNetReference.invokeMethodAsync('SetDateFilterFromDatePicker', {
                    Input: e.element.name,
                    DateStr: dateStr
                });
            }
        });
    },
}

window.Reward = {
    Modal: function () {
        $("#modal_add_reward_transaction").on('show.bs.modal', function (e) {
            GLOBAL.DotNetReference.invokeMethodAsync('OnPurgeAsync',);
        })
    }
}

window.SignUp = {
    Mask: function () {
        $(".phone").mask("(99) 99999-9999");
        $(".document").mask(_getMask, {
            onKeyPress: function (input_value, event, element, options) {
                element.mask(_getMask, options);
            }
        });
    }
}

window.OnModalShow = {
    Focus: function (modal, field) {
        $("#" + modal).on('show.bs.modal', function (e) {
            setTimeout(function () {
                $("input[name=" + field + "]").focus();
            }, 500);
        })
    },
    Select: function (modal, field) {
        $("#" + modal).on('show.bs.modal', function (e) {
            setTimeout(function () {
                $("input[name=" + field + "]").select();
            }, 500);
        })
    }
}

window.Collaborators = {
    Mask: function () {
        $(".phone").mask("(99) 99999-9999");
        $(".document").mask(_getMask, {
            onKeyPress: function (input_value, event, element, options) {
                element.mask(_getMask, options);
            }
        });
    }
}

window.Subscription = {
    PaymentMask: function () {
        $(".securityCode").mask("9999");
        $(".expirationDate").mask("99/99");
        $(".cardNumber").mask("9999 9999 9999 9999");
        $(".document").mask(_getMask, {
            onKeyPress: function (input_value, event, element, options) {
                element.mask(_getMask, options);
            }
        });
    },
    Redirect: function(){
        $("#redirectPaymentLink")[0].click();
    }
}

const _getMask = function (input_value, event, element, options) {
    const numbers = input_value.replace(/\D+/g, '');
    return numbers.length <= 11 ? '000.000.000-000' : '00.000.000/0000-00';
}



toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": true,
    "progressBar": false,
    "positionClass": "toastr-bottom-right",
    "preventDuplicates": true,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};
window.Toastr = {
    Success: function (msg) {
        toastr.success(msg);
    },
    Info: function (msg) {
        toastr.info(msg);
    },
    Error: function (msg) {
        toastr.error(msg);
    },
    Warning: function (msg) {
        toastr.warning(msg);
    }
}

window.Draggable2 = {
    Init: function () {
        var containers = document.querySelectorAll(".draggable-zone");

        if (containers.length === 0) {
            return false;
        }

        var swappable = new Sortable.default(containers, {
            draggable: ".draggable",
            handle: ".draggable .draggable-handle",
            mirror: {
                appendTo: "body",
                constrainDimensions: true
            }
        });

        swappable.on('drag:stop', (evt) => {
            GLOBAL.DotNetReference.invokeMethodAsync('UpdateSortAsync');
        });
    },
    GetSort: function () {
        var items = [];
        $(".card.draggable").each(function (i, item) {
            items.push(Number(item.id))
        })
        return items;
    }
}

loaded = [];
window.Scripts = {
    LoadScript: function (scriptPath) {
        if (loaded[scriptPath]) {
            return;
        }
        return new Promise(function (resolve, reject) {
            var script = document.createElement("script");
            script.src = scriptPath;
            script.type = "text/javascript";
            console.log(scriptPath + " created");

            loaded[scriptPath] = true;

            script.onload = function () {
                console.log(scriptPath + " loaded ok");
                resolve(scriptPath);
            };

            script.onerror = function () {
                console.log(scriptPath + " load failed");
                reject(scriptPath);
            }

            document["body"].appendChild(script);
        });
    }
}

window.Customer = {
    Mask: function(){
        $(".phone").mask("(99) 99999-9999");
        $(".document").mask(_getMask, {
            onKeyPress: function (input_value, event, element, options) {
                element.mask(_getMask, options);
            }
        });
    }
}

window.Address = {
    Mask: function(){
        $(".zipcode").mask("99999-999");
    }
}

window.Store = {
    Masks: function () {
        $(".phone").mask("(99) 99999-9999");
        $(".zipcode").mask("99999-999");
        $(".document").mask(_getMask, {
            onKeyPress: function (input_value, event, element, options) {
                element.mask(_getMask, options);
            }
        });
    },
    GetActiveAvailability: function () {
        return $(".nav-availability .nav-item a.active")[0].text;
    }
}

window.ChartJs = {
    LineChart: function (scores, categories) {
        var a = "300"
        var l = "#000";
        var r = "#d1d1d1";
        var o = "#129fa5";
        var apex = new ApexCharts(document.getElementById("kt_charts_widget_28"), {
            series: [{
                style: {fontFamily: "Inter,Helvetica,sans-serif"},
                name: "Valor",
                data: scores
            }],
            chart: {fontFamily: "Inter,Helvetica,sans-serif", type: "area", height: a, toolbar: {show: !1}},
            legend: {show: true, style: {fontFamily: "Inter,Helvetica,sans-serif"}},
            dataLabels: {enabled: false, style: {fontFamily: "Inter,Helvetica,sans-serif"}},
            fill: {
                type: "gradient",
                gradient: {shadeIntensity: 1, opacityFrom: 1, opacityTo: 1, stops: [0, 80, 100]}
            },
            stroke: {curve: "smooth", show: !0, width: 3, colors: [o]},
            xaxis: {
                categories: categories,
                axisBorder: {show: !1},
                offsetX: 20,
                axisTicks: {show: !1},
                tickAmount: 3,
                labels: {rotate: 0, rotateAlways: !1, style: {colors: l, fontSize: "12px", fontFamily: "Inter,Helvetica,sans-serif"}},
                crosshairs: {position: "front", stroke: {color: o, width: 1, dashArray: 3}},
                tooltip: {enabled: !1, formatter: void 0, offsetY: 0, style: {fontSize: "12px", fontFamily: "Inter,Helvetica,sans-serif"}},
            },
            yaxis: {
                tickAmount: 4,
                labels: {
                    style: {colors: l, fontSize: "12px", fontFamily: "Inter,Helvetica,sans-serif"},
                    formatter: function (e) {
                        return "R$ " + e;
                    },
                },
            },
            states: {
                normal: {filter: {type: "none", value: 0}},
                hover: {filter: {type: "none", value: 0}},
                active: {allowMultipleDataPointsSelection: !1, filter: {type: "none", value: 0}}
            },
            tooltip: {
                style: {fontSize: "12px", fontFamily: "Inter,Helvetica,sans-serif;"},
                y: {
                    formatter: function (e) {
                        return "R$ " + Number(e).toFixed(2).replace(".", ",");
                    },
                },
            },
            colors: [o],
            grid: {borderColor: r, strokeDashArray: 4, yaxis: {lines: {show: !0}}},
            markers: {strokeColor: o, strokeWidth: 3},
        });
        apex.render();
    },
    PineChart: function (values, categories) {
        var t = document.getElementById("kt_pine_chart");
        var e = t.getContext("2d");
        new Chart(e, {
            type: "pie",
            data: {
                datasets: [{
                    data: values,
                    backgroundColor: ["#00FF00", "#FF0000", "#00b2ff", "#FF00FF", "#8A2BE2", "#FFFF00", "#D2691E"]
                }], labels: ["Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo"]
            },
            options: {
                chart: {fontFamily: "inherit"},
                borderWidth: 0,
                cutout: "75%",
                cutoutPercentage: 65,
                responsive: !0,
                maintainAspectRatio: !1,
                title: {display: !1},
                animation: {animateScale: !0, animateRotate: !0},
                stroke: {width: 0},
                tooltips: {
                    enabled: !1,
                    intersect: !1,
                    mode: "nearest",
                    bodySpacing: 5,
                    yPadding: 10,
                    xPadding: 10,
                    caretPadding: 0,
                    displayColors: !1,
                    backgroundColor: "#20D489",
                    titleFontColor: "#ffffff",
                    cornerRadius: 4,
                    footerSpacing: 0,
                    titleSpacing: 0,
                },
                plugins: {legend: {display: !1}},
            },
        });
    },
};

window.ScrollToJs = {
    LastTicketAnswer: function () {
        if ($("#kt_chat_messenger_body .ticket-answer:last")[0]) {
            $("#kt_chat_messenger_body .ticket-answer:last")[0].scrollIntoView({behavior: "smooth"});
        }
        
        return false;
    },
};

