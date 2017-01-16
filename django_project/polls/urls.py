from django.conf.urls import url

from . import views

urlpatterns = [
            url(r'^$', views.index, name='index'),
            url(r'^cosine/(?P<string>\w{0,100})/$', views.cosine , name='cosine'),
            #   url(r'^/(?P<value>\d+)/$',views.index1,name='index1'),
            ]
