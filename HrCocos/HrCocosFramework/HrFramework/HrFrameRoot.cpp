#include "HrFrameRoot.h"
#include "HrFramework/Context/HrContext.h"
#include "HrFramework/View/HrView.h"
#include "HrFramework/Mediator/HrMediator.h"
#include <boost/assert.hpp>


using namespace Hr;

HrFrameRoot::HrFrameRoot()
{

}

HrFrameRoot::~HrFrameRoot()
{

}

void HrFrameRoot::Init()
{
}

void HrFrameRoot::Start()
{
	BOOST_ASSERT(m_pContext);
	m_pContext->MapBindings();
}

void HrFrameRoot::Stop()
{

}
