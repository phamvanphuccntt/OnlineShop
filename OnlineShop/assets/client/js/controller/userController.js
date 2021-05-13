var user = {
    init: function () {
        user.loadProvince();
        user.registerEvent();
    },
    registerEvent: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.loadDistrict(parseInt(id));
            } else {
                $('#ddlDistrict').html('');
            }
        });
        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.loadPrecinct(parseInt(id));
            } else {
                $('#ddlPrecinct').html('');
            }
        });
    },
    loadProvince: function () {
        $.ajax({
            url: '/User/loadProvince',
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status == true) {
                    var data = res.data;
                    var html = '<option>-- Chọn tỉnh thành--</option>';
                    $.each(data, function (i, item) {
                        html += '<option value='+ item.ID +'>'+ item.Name +'</option>';
                    });
                    $('#ddlProvince').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/User/loadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn quận huyện--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadPrecinct: function (id) {
        $.ajax({
            url: '/User/loadPrecinct',
            type: "POST",
            data: {districtID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn phường xã--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlPrecinct').html(html);
                }
            }
        })
    }
}
user.init();