from django.shortcuts import render
from django.http import HttpResponse,JsonResponse
from meta_scripy import getjson,get_how_to
import json

# Create your views here.
def index(request):
    #scrape data here
    #a = json.loads(getjson())
    #print a
    return JsonResponse(getjson(),safe=False)

def cosine(request,string):
    #code
    return JsonResponse(get_how_to(string),safe = False)

    
